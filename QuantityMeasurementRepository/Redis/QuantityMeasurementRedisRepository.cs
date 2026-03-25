// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using Microsoft.EntityFrameworkCore;
// using StackExchange.Redis;

// namespace QuantityMeasurementApp
// {
//     /// <summary>
//     /// Redis-backed implementation of IQuantityMeasurementRepository for logs.
//     /// All log entries are appended to Redis first. The repository then attempts
//     /// to persist them to SQL Server via EF Core. If the database is offline,
//     /// log entries remain stored in Redis until a later attempt can save them.
//     /// </summary>
//     public class QuantityMeasurementRedisRepository : IQuantityMeasurementRepository
//     {
//         private const string AllKey = "qm:logs";
//         private const string PendingKey = "qm:logs:pending";

//         private readonly QuantityMeasurementDbContext dbContext;
//         private readonly IDatabase redis;
//         private readonly JsonSerializerOptions jsonOptions;

//         public QuantityMeasurementRedisRepository(
//             QuantityMeasurementDbContext dbContext,
//             IConnectionMultiplexer connectionMultiplexer)
//         {
//             this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
//             if (connectionMultiplexer == null) throw new ArgumentNullException(nameof(connectionMultiplexer));

//             redis = connectionMultiplexer.GetDatabase();

//             jsonOptions = new JsonSerializerOptions
//             {
//                 WriteIndented = false,
//                 Converters = { new JsonStringEnumConverter() }
//             };

//             InitializeCacheFromDatabaseIfEmpty();
//         }

//         private void InitializeCacheFromDatabaseIfEmpty()
//         {
//             try
//             {
//                 if (!redis.KeyExists(AllKey))
//                 {
//                     List<QuantityMeasurementEntity> allLogs =
//                         dbContext.QuantityMeasurements.AsNoTracking().ToList();

//                     SaveListToRedis(AllKey, allLogs);
//                 }
//             }
//             catch
//             {
//                 // If DB is offline or fails, we simply keep whatever is (or is not) in Redis.
//             }
//         }

//         private List<QuantityMeasurementEntity> LoadListFromRedis(string key)
//         {
//             RedisValue value = redis.StringGet(key);
//             if (value.IsNullOrEmpty)
//             {
//                 return new List<QuantityMeasurementEntity>();
//             }

//             try
//             {
//                 string json = (string)value!;
//                 var list = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json, jsonOptions);
//                 return list ?? new List<QuantityMeasurementEntity>();
//             }
//             catch
//             {
//                 // If cache content is corrupted, start with an empty list for this key.
//                 return new List<QuantityMeasurementEntity>();
//             }
//         }

//         private void SaveListToRedis(string key, List<QuantityMeasurementEntity> list)
//         {
//             try
//             {
//                 string json = JsonSerializer.Serialize(list, jsonOptions);
//                 redis.StringSet(key, json);
//             }
//             catch
//             {
//                 // Do not throw from cache write; keep app behavior stable even if Redis fails.
//             }
//         }

//         public void Save(QuantityMeasurementEntity entity)
//         {
//             if (entity == null) throw new ArgumentNullException(nameof(entity));

//             // 1) Append to Redis cache for ALL logs
//             List<QuantityMeasurementEntity> all = LoadListFromRedis(AllKey);
//             all.Add(entity);
//             SaveListToRedis(AllKey, all);

//             // 2) Try to persist to DB
//             try
//             {
//                 dbContext.QuantityMeasurements.Add(entity);
//                 dbContext.SaveChanges();
//             }
//             catch
//             {
//                 // DB is offline or failed; store in PENDING list to be retried later.
//                 List<QuantityMeasurementEntity> pending = LoadListFromRedis(PendingKey);
//                 pending.Add(entity);
//                 SaveListToRedis(PendingKey, pending);
//                 // We intentionally do not rethrow; the log is at least in Redis.
//             }
//         }

//         public IReadOnlyList<QuantityMeasurementEntity> GetAll()
//         {
//             List<QuantityMeasurementEntity> list = LoadListFromRedis(AllKey);
//             return list.AsReadOnly();
//         }

//         public IReadOnlyList<QuantityMeasurementEntity> GetByOperationType(string operationType)
//         {
//             if (string.IsNullOrWhiteSpace(operationType))
//             {
//                 return Array.Empty<QuantityMeasurementEntity>();
//             }

//             List<QuantityMeasurementEntity> list = LoadListFromRedis(AllKey)
//                 .Where(e => string.Equals(e.OperationType, operationType, StringComparison.OrdinalIgnoreCase))
//                 .ToList();

//             return list.AsReadOnly();
//         }

//         public IReadOnlyList<QuantityMeasurementEntity> GetByMeasurementCategory(MeasurementCategory category)
//         {
//             List<QuantityMeasurementEntity> list = LoadListFromRedis(AllKey)
//                 .Where(e => e.Category.HasValue && e.Category.Value == category)
//                 .ToList();

//             return list.AsReadOnly();
//         }

//         public int GetTotalCount()
//         {
//             List<QuantityMeasurementEntity> list = LoadListFromRedis(AllKey);
//             return list.Count;
//         }

//         public void DeleteAll()
//         {
//             // Clear Redis
//             redis.KeyDelete(AllKey);
//             redis.KeyDelete(PendingKey);

//             // Clear DB
//             try
//             {
//                 dbContext.QuantityMeasurements.RemoveRange(dbContext.QuantityMeasurements);
//                 dbContext.SaveChanges();
//             }
//             catch
//             {
//                 // If DB is offline, we at least cleared Redis; DB will be out-of-sync until next sync.
//             }
//         }

//         public string GetPoolStatistics()
//         {
//             int count = GetTotalCount();
//             int pendingCount = LoadListFromRedis(PendingKey).Count;

//             return $"Redis-backed log repository. Cached logs: {count}, pending for DB: {pendingCount}.";
//         }

//         public void ReleaseResources()
//         {
//             // DbContext is scoped and disposed by DI; Redis connection is singleton via DI.
//         }
//     }
// }