using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Database-backed implementation of IQuantityMeasurementRepository using SQL Server and ADO.NET.
    /// </summary>
    public sealed class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly DbConnectionPool connectionPool;

        public QuantityMeasurementDatabaseRepository(string connectionString, int maxPoolSize = 20)
        {
            connectionPool = new DbConnectionPool(connectionString, maxPoolSize);
            EnsureSchema();
        }

        private void EnsureSchema()
        {
            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuantityMeasurements]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[QuantityMeasurements] (
                        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                        [TimestampUtc] DATETIME2 NOT NULL,
                        [OperationType] NVARCHAR(100) NOT NULL,
                        [Details] NVARCHAR(MAX) NOT NULL,
                        [HasError] BIT NOT NULL,
                        [ErrorMessage] NVARCHAR(MAX) NULL,
                        [Category] INT NULL
                    );
                END";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error ensuring database schema for QuantityMeasurements.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while ensuring database schema for QuantityMeasurements.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO [dbo].[QuantityMeasurements]
                (Id, TimestampUtc, OperationType, Details, HasError, ErrorMessage, Category)
                VALUES
                (@Id, @TimestampUtc, @OperationType, @Details, @HasError, @ErrorMessage, @Category);";

                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@TimestampUtc", entity.Timestamp);
                command.Parameters.AddWithValue("@OperationType", (object?)entity.OperationType ?? DBNull.Value);
                command.Parameters.AddWithValue("@Details", (object?)entity.Details ?? DBNull.Value);
                command.Parameters.AddWithValue("@HasError", entity.HasError);
                command.Parameters.AddWithValue("@ErrorMessage", (object?)entity.ErrorMessage ?? DBNull.Value);
                if (entity.Category.HasValue)
                {
                    command.Parameters.AddWithValue("@Category", (int)entity.Category.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@Category", DBNull.Value);
                }

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error saving quantity measurement entity to database.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while saving quantity measurement entity to database.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            var result = new List<QuantityMeasurementEntity>();
            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, TimestampUtc, OperationType, Details, HasError, ErrorMessage, Category
                FROM [dbo].[QuantityMeasurements]
                ORDER BY TimestampUtc;";
                command.CommandType = CommandType.Text;

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(MapEntity(reader));
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error retrieving all quantity measurements from database.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while retrieving all quantity measurements from database.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }

            return result;
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperationType(string operationType)
        {
            var result = new List<QuantityMeasurementEntity>();
            if (string.IsNullOrWhiteSpace(operationType))
            {
                return result.AsReadOnly();
            }

            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, TimestampUtc, OperationType, Details, HasError, ErrorMessage, Category
                FROM [dbo].[QuantityMeasurements]
                WHERE OperationType = @OperationType
                ORDER BY TimestampUtc;";
                command.Parameters.AddWithValue("@OperationType", operationType);
                command.CommandType = CommandType.Text;

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(MapEntity(reader));
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error retrieving measurements by operation type from database.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while retrieving measurements by operation type from database.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }

            return result;
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByMeasurementCategory(MeasurementCategory category)
        {
            var result = new List<QuantityMeasurementEntity>();
            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, TimestampUtc, OperationType, Details, HasError, ErrorMessage, Category
                FROM [dbo].[QuantityMeasurements]
                WHERE Category = @Category
                ORDER BY TimestampUtc;";
                command.Parameters.AddWithValue("@Category", (int)category);
                command.CommandType = CommandType.Text;

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(MapEntity(reader));
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error retrieving measurements by measurement category from database.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while retrieving measurements by measurement category from database.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }

            return result;
        }

        public int GetTotalCount()
        {
            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM [dbo].[QuantityMeasurements];";
                command.CommandType = CommandType.Text;

                object? scalar = command.ExecuteScalar();
                return Convert.ToInt32(scalar);
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error getting total count of measurements from database.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while getting total count of measurements from database.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }
        }

        public void DeleteAll()
        {
            SqlConnection? connection = null;
            try
            {
                connection = connectionPool.Rent();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM [dbo].[QuantityMeasurements];";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Error deleting all measurements from database.", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Unexpected error while deleting all measurements from database.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connectionPool.Return(connection);
                }
            }
        }

        public string GetPoolStatistics()
        {
            return connectionPool.GetStatistics();
        }

        public void ReleaseResources()
        {
            connectionPool.Dispose();
        }

        private static QuantityMeasurementEntity MapEntity(SqlDataReader reader)
        {
            Guid id = reader.GetGuid(reader.GetOrdinal("Id"));
            DateTime timestamp = reader.GetDateTime(reader.GetOrdinal("TimestampUtc"));
            string operationType = reader.GetString(reader.GetOrdinal("OperationType"));
            string details = reader.GetString(reader.GetOrdinal("Details"));
            bool hasError = reader.GetBoolean(reader.GetOrdinal("HasError"));

            string? errorMessage = reader.IsDBNull(reader.GetOrdinal("ErrorMessage"))
                ? null
                : reader.GetString(reader.GetOrdinal("ErrorMessage"));

            MeasurementCategory? category = null;
            int categoryOrdinal = reader.GetOrdinal("Category");
            if (!reader.IsDBNull(categoryOrdinal))
            {
                int categoryValue = reader.GetInt32(categoryOrdinal);
                category = (MeasurementCategory)categoryValue;
            }

            return new QuantityMeasurementEntity(
                id,
                timestamp,
                operationType,
                details,
                hasError,
                errorMessage,
                category);
        }
    }
}