using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemoApi.Model
{
    public class JsonData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public BsonDocument Data { get; set; }
    }
}