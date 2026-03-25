using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Redis-backed repository for quantity operations with SQL Server as backing store.
    /// All operations are appended to Redis first. The repository then attempts to
    /// persist them to the database. If the database is offline, operations remain
    /// stored in Redis until a later attempt can save them.
    /// </summary>
    public class QuantityOperationRedisRepository : IQuantityOperationRepository
    {
        private const string AllKey = "qm:operations";
        private const string PendingKey = "qm:operations:pending";

        private readonly QuantityMeasurementDbContext dbContext;
        private readonly IDatabase redis;
        private readonly JsonSerializerOptions jsonOptions;

        public QuantityOperationRedisRepository(
            QuantityMeasurementDbContext dbContext,
            IConnectionMultiplexer connectionMultiplexer)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            if (connectionMultiplexer == null) throw new ArgumentNullException(nameof(connectionMultiplexer));

            redis = connectionMultiplexer.GetDatabase();

            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = { new JsonStringEnumConverter() }
            };

            InitializeCacheFromDatabaseIfEmpty();
        }

        private void InitializeCacheFromDatabaseIfEmpty()
        {
            try
            {
                if (!redis.KeyExists(AllKey))
                {
                    List<QuantityOperation> allOps =
                        dbContext.QuantityOperations.AsNoTracking().ToList();

                    SaveListToRedis(AllKey, allOps);
                }
            }
            catch
            {
                // If DB is offline, we simply keep any existing Redis data (or start empty).
            }
        }

        private List<QuantityOperation> LoadListFromRedis(string key)
        {
            RedisValue value = redis.StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return new List<QuantityOperation>();
            }

            try
            {
                string json = (string)value!;
                var list = JsonSerializer.Deserialize<List<QuantityOperation>>(json, jsonOptions);
                return list ?? new List<QuantityOperation>();
            }
            catch
            {
                // If cache content is corrupted, start with an empty list for this key.
                return new List<QuantityOperation>();
            }
        }

        private void SaveListToRedis(string key, List<QuantityOperation> list)
        {
            try
            {
                string json = JsonSerializer.Serialize(list, jsonOptions);
                redis.StringSet(key, json);
            }
            catch
            {
                // Do not throw from cache write; keep app behavior as stable as possible.
            }
        }

        public async Task SaveAsync(QuantityOperation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            // 1) Append to in-memory cache in Redis (ALL operations)
            List<QuantityOperation> all = LoadListFromRedis(AllKey);
            all.Add(operation);
            SaveListToRedis(AllKey, all);

            // 2) Try to persist to DB
            try
            {
                dbContext.QuantityOperations.Add(operation);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR saving operation to DB: " + ex.Message);

                // DB is offline or failed. Store in PENDING list to be retried later.
                List<QuantityOperation> pending = LoadListFromRedis(PendingKey);
                pending.Add(operation);
                SaveListToRedis(PendingKey, pending);
                // We intentionally do not throw; the operation is at least in Redis.
            }
        }

        public Task<IReadOnlyList<QuantityOperation>> GetAllAsync()
        {
            List<QuantityOperation> list = LoadListFromRedis(AllKey);
            return Task.FromResult((IReadOnlyList<QuantityOperation>)list.AsReadOnly());
        }

        public Task<IReadOnlyList<QuantityOperation>> GetByOperationTypeAsync(string operationType)
        {
            if (string.IsNullOrWhiteSpace(operationType))
            {
                return Task.FromResult((IReadOnlyList<QuantityOperation>)Array.Empty<QuantityOperation>());
            }

            List<QuantityOperation> list = LoadListFromRedis(AllKey)
                .Where(o => string.Equals(o.OperationType, operationType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Task.FromResult((IReadOnlyList<QuantityOperation>)list.AsReadOnly());
        }

        public Task<IReadOnlyList<QuantityOperation>> GetByCategoryAsync(MeasurementCategory category)
        {
            List<QuantityOperation> list = LoadListFromRedis(AllKey)
                .Where(o => o.Category == category)
                .ToList();

            return Task.FromResult((IReadOnlyList<QuantityOperation>)list.AsReadOnly());
        }
    }
}