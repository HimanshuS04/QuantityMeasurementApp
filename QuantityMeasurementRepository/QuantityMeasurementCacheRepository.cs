// QuantityMeasurementRepository/QuantityMeasurementCacheRepository.cs
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
            // JSON file stored next to the executable (bin/Debug/net10.0)
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
                    // If deserialize fails (corrupt file), start fresh
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
                // Optional: log to console; don't crash the app
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
    }
}