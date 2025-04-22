using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace TestUtils.BSON;

public static class BsonConvertionHelper
{
    public static byte[] ConvertJsonToBytes(string json)
    {
        BsonDocument doc = BsonDocument.Parse(json);

        byte[] bsonBytes;
        using (var stream = new MemoryStream())
        using (var writer = new BsonBinaryWriter(stream))
        {
            BsonSerializer.Serialize(writer, doc);
            bsonBytes = stream.ToArray();
        }
        
        return bsonBytes;

    }
}

