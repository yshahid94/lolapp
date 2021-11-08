using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace lolappAPI.Types
{
    [BsonCollection("league")]
    public class League
    {
        [BsonElement("leagueId")]
        public string LeagueID { get; set; }
        [BsonElement("queueType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public QueueType QueueType { get; set; }
        [BsonElement("tier")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Tier Tier { get; set; }
        [BsonElement("rank")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Rank Rank { get; set; }
        [BsonElement("summonerId")]
        public string SummonerID { get; set; }
        [BsonElement("summonerName")]
        public string SummonerName { get; set; }
        [BsonElement("leaguePoints")]
        public int LeaguePoints { get; set; }
        [BsonElement("wins")]
        public int Wins { get; set; }
        [BsonElement("losses")]
        public int Losses { get; set; }
    }
    public enum QueueType
    {
        Ranked_Solo_5x5,
        Ranked_Flex_SR,
        Unknown
    }
    public enum Tier
    {
        Iron = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        Unknown
    }
    public enum Rank
    {
        I = 1,
        II = 2,
        III = 3,
        IV = 4,
        Unknown
    }
}
