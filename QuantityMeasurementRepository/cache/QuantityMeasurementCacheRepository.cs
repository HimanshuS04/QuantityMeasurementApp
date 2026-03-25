using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuantityMeasurementApp
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly Lazy<QuantityMeasurementCacheRepository> lazyInstance =
            new Lazy<QuantityMeasurementCacheRepository>(() => new QuantityMeasurementCacheRepository());

        public static QuantityMeasurementCacheRepository Instance => lazyInstance.Value;

        private readonly List<QuantityMeasurementEntity> entities;
        private readonly string filePath;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private QuantityMeasurementCacheRepository()
        {
            // JSON file will live next to the executable (bin/Debug/net10.0)
            filePath = Path.Combine(AppContext.BaseDirectory, "measurements_cache.json");

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var loaded = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json, JsonOptions);
                    entities = loaded ?? new List<QuantityMeasurementEntity>();
                }
                catch
                {
                    // If the file is corrupted or unreadable, start with an empty list
                    entities = new List<QuantityMeasurementEntity>();
                }
            }
            else
            {
                entities = new List<QuantityMeasurementEntity>();
            }
        }

        private void PersistToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(entities, JsonOptions);
                File.WriteAllText(filePath, json);
            }
            catch
            {
                // You can optionally log to console here if you want,
                // but don't crash the app just because logging persistence failed.
            }
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity != null)
            {
                entities.Add(entity);
                PersistToFile();
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            return entities.AsReadOnly();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperationType(string operationType)
        {
            if (string.IsNullOrWhiteSpace(operationType))
            {
                return Array.Empty<QuantityMeasurementEntity>();
            }

            return entities
                .Where(e => string.Equals(e.OperationType, operationType, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByMeasurementCategory(MeasurementCategory category)
        {
            return entities
                .Where(e => e.Category.HasValue && e.Category.Value == category)
                .ToList()
                .AsReadOnly();
        }

        public int GetTotalCount()
        {
            return entities.Count;
        }

        public void DeleteAll()
        {
            entities.Clear();
            PersistToFile();
        }

        public string GetPoolStatistics()
        {
            // No DB pool here, but we still return some diagnostic info
            return $"In-memory + JSON repository. Stored items: {entities.Count}. No DB connections.";
        }

        public void ReleaseResources()
        {
            // Nothing special to release for cache + JSON
        }
    }
}