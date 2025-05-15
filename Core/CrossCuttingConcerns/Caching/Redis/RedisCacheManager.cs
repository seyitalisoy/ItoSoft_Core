
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheManager()
        {
            _redis = ServiceTool.ServiceProvider.GetService<IConnectionMultiplexer>();
            _db = _redis.GetDatabase();
        }

        public void Set(string key, object value, int duration)
        {
            var jsonData = JsonConvert.SerializeObject(value);
            _db.StringSet(key, jsonData, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            var data = _db.StringGet(key);
            return data.HasValue
                ? JsonConvert.DeserializeObject<T>(data)
                : default;
        }

        public bool IsAdd(string key)
        {
            return _db.KeyExists(key);
        }

        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {

            var endpoints = _redis.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                if (!server.IsConnected) continue;

                var keys = server.Keys(pattern: "*") 
                    .Where(k => System.Text.RegularExpressions.Regex.IsMatch(k, pattern));

                foreach (var key in keys)
                {
                    _db.KeyDelete(key);
                }
            }
        }
    }

}
