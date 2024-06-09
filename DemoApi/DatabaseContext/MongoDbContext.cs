using DemoApi.Model;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
namespace DemoApi.DatabaseContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        IOptions<MongoDbSettings> _setting;
        public MongoDbContext(IOptions<MongoDbSettings> setting)
        {
            var client = new MongoClient(setting.Value.ConnectionString);
            var database = client.GetDatabase(setting.Value.Database);

           // var _collection = database.GetCollection<JsonData>(setting.Value.Collection);//Not schema of JsonData insteda we will use BsonDocument  which we cab work dynamically
            _database = database;
            _setting = setting;
        }
        public IMongoCollection<BsonDocument> Records => _database.GetCollection<BsonDocument>(_setting.Value.Collection);
     
    }
}
