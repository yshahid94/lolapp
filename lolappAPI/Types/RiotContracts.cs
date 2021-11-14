using lolappAPI.Repository;
using Newtonsoft.Json;

namespace lolappAPI.Types
{
    #region Messages

        #region Outbound
        public class GetSummonerByNameOutboundMessage : RiotOutboundMessage
        {
            public string Name { get; set; }
            public override string OrderURL { get { return "lol/summoner/v4/summoners/by-name/" + Name; } }
            public GetSummonerByNameOutboundMessage()
            {
                base.CreateMessage(
                    RiotOutboundMessage.MessageType_Enum.GET,
                    typeof(GetSummonerInboundMessage),
                    RiotAPIURLBaseVersion.Old);
            }
        }
        public class GetLeagueByEncryptedSummonerIDOutboundMessage : RiotOutboundMessage
        {
            public string EncryptedSummonerId { get; set; }
            public override string OrderURL { get { return "lol/league/v4/entries/by-summoner/" + EncryptedSummonerId; } }
            public GetLeagueByEncryptedSummonerIDOutboundMessage()
            {
                base.CreateMessage(
                    RiotOutboundMessage.MessageType_Enum.GET,
                    typeof(GetLeagueBySummonerInboundMessage),
                    RiotAPIURLBaseVersion.Old);
            }
        }
        #endregion

        #region Inbound
        public class RiotInboundMessage
        {
        }
        public class GetSummonerInboundMessage : RiotInboundMessage
        {
            [JsonProperty("id")]
            public string ID { get; set; }
            [JsonProperty("accountId")]
            public string AccountID { get; set; }
            [JsonProperty("puuid")]
            public string PUUID { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("profileIconId")]
            public int ProfileIconID { get; set; }
            [JsonProperty("revisionDate")]
            [JsonConverter(typeof(MillisecondEpochConverter))]
            public DateTime RevisionDate { get; set; }
            [JsonProperty("summonerLevel")]
            public int SummonerLevel { get; set; }

        }
        public class GetLeagueBySummonerInboundMessage : RiotInboundMessage
        {
            public List<RiotAPILeague> array { get; set; }
        }
        public class ErrorMessage : RiotInboundMessage
        {
            [JsonProperty("status")]
            public MessageStatus Status { get; set; }

    }
    #endregion

    #endregion

    #region Types
    public class RiotAPILeague 
    {
        [JsonProperty("leagueId")]
        public string LeagueID { get; set; }
        [JsonProperty("queueType")]
        public string QueueType { get; set; }
        [JsonProperty("tier")]
        public string Tier { get; set; }
        [JsonProperty("Rank")]
        public string Rank { get; set; }
        [JsonProperty("SummonerID")]
        public string SummonerID { get; set; }
        [JsonProperty("SummonerName")]
        public string SummonerName { get; set; }
        [JsonProperty("LeaguePoints")]
        public int LeaguePoints { get; set; }
        [JsonProperty("Wins")]
        public int Wins { get; set; }
        [JsonProperty("Losses")]
        public int Losses { get; set; }
        [JsonProperty("Veteran")]
        public bool Veteran { get; set; }
        [JsonProperty("Inactive")]
        public string Inactive { get; set; }
        [JsonProperty("FreshBlood")]
        public bool FreshBlood { get; set; }
        [JsonProperty("HotStreak")]
        public bool HotStreak { get; set; }
    }

    public class MessageStatus 
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("status_code")]
        public int intStatusCode { get; set; }
        [JsonIgnore()]
        public System.Net.HttpStatusCode StatusCode { get; set; }
    }
    #endregion

    #region Enum
    public enum RiotAPIURLBaseVersion
    {
        Old,
        New
    }
    #endregion
}
