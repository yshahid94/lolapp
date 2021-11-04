using MongoDB.Bson.Serialization.Attributes;

namespace lolappAPI.Types
{
    [BsonCollection("users")]
    public class User : Document
    {
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("rank")]
        public string Rank { get; set; }
    }
}
