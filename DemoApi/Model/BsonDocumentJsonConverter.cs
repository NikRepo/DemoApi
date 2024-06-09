namespace DemoApi.Model;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using Newtonsoft.Json.Linq;

public class BsonDocumentJsonConverter : JsonConverter<BsonDocument>
{

    public override BsonDocument? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, BsonDocument? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == Newtonsoft.Json.JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType == Newtonsoft.Json.JsonToken.StartObject)
        {
            var document = new BsonDocument();
            serializer.Populate(reader, document);
            return document;
        }
        throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, BsonDocument? value, JsonSerializer serializer)
    {
        var json = value.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
        writer.WriteRawValue(json);
    }
}
