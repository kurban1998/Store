using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using ThisrtApiService.Models;
using TshisrtApiService;
using IServer = StackExchange.Redis.IServer;

namespace ThisrtApiService
{
    public class RedisService : IRedisService
    {
        public RedisService(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _server = redis.GetServer(connectionString);
            _database = redis.GetDatabase();
        }

        public void SaveTshirt(TshirtDTO tshirt)
        {
            var jsonData = JsonConvert.SerializeObject(tshirt);
            if (!_database.KeyExists(tshirt.TshirtId.ToString()))
            {
                _database.StringSet(tshirt.TshirtId.ToString(), jsonData);
            }
        }

        public TshirtDTO GetTshirt(int id)
        {
            var jsonData = _database.StringGet(id.ToString());
            return jsonData.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<TshirtDTO>(jsonData);
        }

        public IList<TshirtDTO> GetTshirts()
        {
            var keys = _server.Keys();

            if(!keys.Any())
            {
                return new List<TshirtDTO>();
            }

            var tshirts = new List<TshirtDTO>();
            foreach (var key in keys)
            {
                var value = _database.StringGet(key);
                tshirts.Add(JsonConvert.DeserializeObject<TshirtDTO>(value));
            }

            return tshirts;
        }

        public void DeleteByid(int tshirtId)
        {
            _database.KeyDelete(tshirtId.ToString());
        }

        private readonly IServer _server;
        private readonly IDatabase _database;
    }
}
