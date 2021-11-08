﻿using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;

namespace lolappAPI.Repository
{
    public class LeagueRepository : ILeagueRepository
    {
        private IConfiguration _configuration;
        public LeagueRepository(IConfiguration config)
        {
            _configuration = config;
        }
        public List<League> GetLeaguesByEncryptedSummonerID(string encryptedSummonerID)
        {
            List<RiotInboundMessage> responses = new List<RiotInboundMessage>();

            GetLeagueByEncryptedSummonerIDOutboundMessage req = new GetLeagueByEncryptedSummonerIDOutboundMessage();
            req.EncryptedSummonerId = encryptedSummonerID;

            var restAPI = new RiotRestAPI();

            GetLeagueBySummonerInboundMessage response = (GetLeagueBySummonerInboundMessage)RiotRestAPI.SendMessage(req, restAPI);


            List<League> leagues = new List<League>();

            foreach (var respLeague in response.array)
            {
                League league = new League();

                league.LeagueID = respLeague.LeagueID;
                league.QueueType = QueueTypeStringToEnum(respLeague.QueueType);
                league.Tier = TierStringToEnum(respLeague.Tier);
                league.Rank = RankStringToEnum(respLeague.Rank);
                league.SummonerID = respLeague.SummonerID;
                league.SummonerName = respLeague.SummonerName;
                league.LeaguePoints = respLeague.LeaguePoints;
                league.Wins = respLeague.Wins;
                league.Losses = respLeague.Losses;

                leagues.Add(league);
            }

            return leagues;

        }
        private Tier TierStringToEnum(string tier)
        {
            switch (tier)
            {
                case "IRON":
                    return Tier.Iron;
                case "SILVER":
                    return Tier.Silver;
                case "GOLD":
                    return Tier.Gold;
                case "PLATINUM":
                    return Tier.Platinum;
                case "DIAMOND":
                    return Tier.Diamond;
                default:
                    return Tier.Unknown;
            }
        }
        private QueueType QueueTypeStringToEnum(string queueType)
        {
            switch (queueType)
            {
                case "RANKED_SOLO_5X5":
                    return QueueType.Ranked_Solo_5x5;
                case "RANKED_FLEX_SR":
                    return QueueType.Ranked_Flex_SR;
                default:
                    return QueueType.Unknown;
            }
        }
        private Rank RankStringToEnum(string rank)
        {
            switch (rank)
            {
                case "I":
                    return Rank.I;
                case "II":
                    return Rank.II;
                case "III":
                    return Rank.III;
                case "IV":
                    return Rank.IV;
                default:
                    return Rank.Unknown;
            }
        }
    }
}
