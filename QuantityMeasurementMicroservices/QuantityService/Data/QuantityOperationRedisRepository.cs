using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using QuantityService.Models;
using StackExchange.Redis;

namespace QuantityService.Data
{
    /// <summary>
    /// Redis-backed repository for quantity operations.
    /// Disconnected architecture:
    /// - Writes to Redis first (always succeeds if Redis available)
    /// - Then tries to persist to SQL Server
    /// - If DB is offline, saves to Redis pending list
    /// - Pending list can be retried when DB comes back online
    /// </summary>
    public class QuantityOperationRedisRepository : IQuantityOperationRepository
    {
        private const string AllKey = "qm:operations";
        private const string PendingKey = "qm:operations:pending";

        private readonly QuantityDbContext dbContext;
        private readonly IDatabase redis;
        private readonly JsonSerializerOptions jsonOptions;

        public QuantityOperationRedisRepository(
            QuantityDbContext dbContext,
            IConnectionMultiplexer connectionMultiplexer)
        {
            this.dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));

            if (connectionMultiplexer == null)
                throw new ArgumentNullException(nameof(connectionMultiplexer));

            redis = connectionMultiplexer.GetDatabase();

            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                Converters = { new JsonStringEnumConverter() }
            };

            InitializeCacheFromDatabaseIfEmpty();
        }

        // On startup: load existing DB operations into Redis if Redis is empty
        private void InitializeCacheFromDatabaseIfEmpty()
        {
            try
            {
                if (!redis.KeyExists(AllKey))
                {
                    List<QuantityOperation> allOps =
                        dbContext.QuantityOperations.AsNoTracking().ToList();

                    SaveListToRedis(AllKey, allOps);
                    Console.WriteLine($"Redis cache initialized with {allOps.Count} operations from DB.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cache initialization failed: " + ex.Message);
            }
        }

        private List<QuantityOperation> LoadListFromRedis(string key)
        {
            try
            {
                RedisValue value = redis.StringGet(key);
                if (value.IsNullOrEmpty)
                    return new List<QuantityOperation>();

                string json = (string)value!;
                var list = JsonSerializer.Deserialize<List<QuantityOperation>>(json, jsonOptions);
                return list ?? new List<QuantityOperation>();
            }
            catch
            {
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
            catch (Exception ex)
            {
                Console.WriteLine("Redis write failed: " + ex.Message);
            }
        }

        public async Task SaveAsync(QuantityOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            // Step 1: Always write to Redis first
            List<QuantityOperation> all = LoadListFromRedis(AllKey);
            all.Add(operation);
            SaveListToRedis(AllKey, all);
            Console.WriteLine($"Operation saved to Redis cache. Total cached: {all.Count}");

            // Step 2: Try to persist to SQL Server
            try
            {
                dbContext.QuantityOperations.Add(operation);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Operation persisted to SQL Server successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB offline or error: " + ex.Message);
                Console.WriteLine("Operation added to Redis pending list for later retry.");

                // Step 3: DB offline — save to pending list for retry
                List<QuantityOperation> pending = LoadListFromRedis(PendingKey);
                pending.Add(operation);
                SaveListToRedis(PendingKey, pending);
            }
        }

        public Task<IReadOnlyList<QuantityOperation>> GetAllAsync()
        {
            List<QuantityOperation> list = LoadListFromRedis(AllKey);
            return Task.FromResult((IReadOnlyList<QuantityOperation>)list.AsReadOnly());
        }

        public Task<IReadOnlyList<QuantityOperation>> GetByUserIdAsync(int userId)
        {
            List<QuantityOperation> list = LoadListFromRedis(AllKey)
                .Where(o => o.UserId.HasValue && o.UserId.Value == userId)
                .ToList();
            return Task.FromResult((IReadOnlyList<QuantityOperation>)list.AsReadOnly());
        }

        public Task<IReadOnlyList<QuantityOperation>> GetByOperationTypeAsync(string operationType)
        {
            if (string.IsNullOrWhiteSpace(operationType))
                return Task.FromResult((IReadOnlyList<QuantityOperation>)Array.Empty<QuantityOperation>());

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