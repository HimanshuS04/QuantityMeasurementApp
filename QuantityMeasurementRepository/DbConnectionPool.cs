using System;
using System.Collections.Concurrent;
using System.Data;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Simple connection pool wrapper around SqlConnection with basic statistics.
    /// </summary>
    public sealed class DbConnectionPool : IDisposable
    {
        private readonly string connectionString;
        private readonly int maxSize;
        private readonly ConcurrentBag<SqlConnection> pool = new();
        private int createdConnections;
        private bool disposed;

        public DbConnectionPool(string connectionString, int maxSize)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            if (maxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxSize), "Max pool size must be positive.");

            this.connectionString = connectionString;
            this.maxSize = maxSize;
        }

        public SqlConnection Rent()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(DbConnectionPool));

            if (pool.TryTake(out SqlConnection? connection))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }

            int newCount = Interlocked.Increment(ref createdConnections);
            if (newCount <= maxSize)
            {
                try
                {
                    var conn = new SqlConnection(connectionString);
                    conn.Open();
                    return conn;
                }
                catch
                {
                    Interlocked.Decrement(ref createdConnections);
                    throw;
                }
            }

            Interlocked.Decrement(ref createdConnections);
            throw new DatabaseException("Connection pool exhausted. No more connections can be created.");
        }

        public void Return(SqlConnection? connection)
        {
            if (connection == null || disposed)
            {
                connection?.Dispose();
                return;
            }

            if (connection.State == ConnectionState.Open)
            {
                pool.Add(connection);
            }
            else
            {
                connection.Dispose();
                Interlocked.Decrement(ref createdConnections);
            }
        }

        public string GetStatistics()
        {
            return $"Connections created: {createdConnections}, Available in pool: {pool.Count}, Max pool size: {maxSize}";
        }

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;

            while (pool.TryTake(out SqlConnection? connection))
            {
                try
                {
                    connection.Dispose();
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}