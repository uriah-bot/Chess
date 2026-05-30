using System.Text.Json;

namespace Chess.Data
{
    public interface IJSONRepository<TKey, TValue>
    {
        Dictionary<string, Dictionary<string, TValue>> Cache { get; }
        Task SaveToJSONAsync(string filePath, TKey key, TValue value);
        Task<TValue> FetchFromJSONAsync(string filePath, TKey key);
        Task DeleteFromJSONAsync(string filePath, TKey key);
    }

    public class JSONRepository<TKey, TValue> : IJSONRepository<TKey, TValue>
    {
        private static readonly string _baseDirectory = "JSON";

        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public Dictionary<string, Dictionary<string, TValue>> Cache { get; } = new Dictionary<string, Dictionary<string, TValue>>();

        public async Task DeleteFromJSONAsync(string path, TKey key)
        {
            var filePath = Path.Combine(_baseDirectory, path);

            var data = await LoadDictionary(filePath);
            if (data.Remove(key.ToString()))
            {
                await SaveDictionary(filePath, data);
            }
        }

        public async Task<TValue> FetchFromJSONAsync(string path, TKey key)
        {
            var filePath = Path.Combine(_baseDirectory, path);

            string lookupKey = key.ToString();

            if (!Cache.ContainsKey(filePath))
            {
                Cache[filePath] = await LoadDictionary(filePath);
            }

            if (Cache[filePath].TryGetValue(lookupKey, out var value))
            {
                return value;
            }

            return default;
        }

        public async Task SaveToJSONAsync(string path, TKey key, TValue value)
        {
            var filePath = Path.Combine(_baseDirectory, path);

            var data = await LoadDictionary(filePath);
            data[key.ToString()] = value;
            await SaveDictionary(filePath, data);
        }


        // helpers
        private async Task<Dictionary<string, TValue>> LoadDictionary(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, TValue>();
            }

            // opens and reads the JSON file
            using FileStream openStream = File.OpenRead(filePath);
            if (openStream.Length == 0) return new Dictionary<string, TValue>();

            // deserializes the JSON into a dictionary of string, Tvalue
            return await JsonSerializer.DeserializeAsync<Dictionary<string, TValue>>(openStream, _options)
                   ?? new Dictionary<string, TValue>();
        }

        private async Task SaveDictionary(string filePath, Dictionary<string, TValue> data)
        {
            // creates/overwrites the file in the path
            using FileStream createStream = File.Create(filePath);
            
            // serialize the objects into the JSON
            await JsonSerializer.SerializeAsync(createStream, data, _options);
        }
    }
}
