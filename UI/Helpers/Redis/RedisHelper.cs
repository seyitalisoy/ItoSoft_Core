using Newtonsoft.Json;
using StackExchange.Redis;
using UI.Models.Cart;

namespace UI.Helpers.Redis
{
    public class RedisHelper
    {
        private readonly IDatabase _database;

        public RedisHelper(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        private string GetCartKey(string userId) => $"cart:{userId}";

        public List<CartItem> GetCart(string userId)
        {
            var data = _database.StringGet(GetCartKey(userId));
            return data.IsNullOrEmpty ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(data);
        }

        public void SetCart(string userId, List<CartItem> cart)
        {
            var data = JsonConvert.SerializeObject(cart);
            _database.StringSet(GetCartKey(userId), data, TimeSpan.FromHours(2)); // 2 saat saklama süresi
        }

        public void ClearCart(string userId)
        {
            _database.KeyDelete(GetCartKey(userId));
        }
    }
}
