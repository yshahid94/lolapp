using MongoDB.Bson.Serialization.Attributes;

namespace lolappAPI.Types
{
    [BsonCollection("summoners")]
    public class Summoner : Document
    {
        [BsonElement("summonerId")]
        public string SummonerID { get; set; }
        [BsonElement("accountId")]
        public string AccountID { get; set; }
        [BsonElement("puuId")]
        public string PUUID { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("profileIconId")]
        public int ProfileIconID { get; set; }
        [BsonElement("revisionDate")]
        public DateTime RevisionDate { get; set; }
        [BsonElement("summonerLevel")]
        public int SummonerLevel { get; set; }
    }
}
