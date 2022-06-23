using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace lolappAPI.Repository
{
    public class LeagueRepository : ILeagueRepository
    {
        private IConfiguration _config;
        public LeagueRepository(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets the current snapshots for given summoner saves them to the db
        /// </summary>
        /// <returns>The list of current snapshots for the given summoner</returns>
        public List<League> GetLeaguesByEncryptedSummonerIDFromRiotAndSaveToDB(string encryptedSummonerID)
        {
            //Get leagues from RiotAPI
            List<League> leagues = GetLeaguesByEncryptedSummonerIDFromRiot(encryptedSummonerID);
            //Save those leagues to database
            return SaveLeaguesToDB(leagues);
        }
        /// <summary>
        /// Gets the current snapshots for all the given summoner's leagues (Solo, Flex) via RiotAPI and returns them
        /// </summary>
        /// <returns>List of current league snapshots for all summoners</returns>
        private List<League> GetLeaguesByEncryptedSummonerIDFromRiot(string encryptedSummonerID)
        {
            List<RiotInboundMessage> responses = new List<RiotInboundMessage>();

            GetLeagueByEncryptedSummonerIDOutboundMessage req = new GetLeagueByEncryptedSummonerIDOutboundMessage();
            req.EncryptedSummonerId = encryptedSummonerID;

            var restAPI = new RiotRestAPI(_config);

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
        /// <summary>
        /// Saves the list of league snapshots given to the db and returns them with the inserted ids
        /// </summary>
        /// <returns>The list of snapshots given in as the parameter plus their inserted ids and created dates</returns>
        public List<League> SaveLeaguesToDB(List<League> leagues)
        {
            DataAccessRepository<League> dar = new DataAccessRepository<League>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());
            
            if(leagues.Count > 0)
            {
                dar.InsertMany(leagues);
            }

            return leagues;
        }

        //public List<League> GetLeaguesForSummonersAndQueueType(List<string> encryptedSummonerIDs, QueueType? queueType = null)
        //{

        //}
        //private League GetLatestLeagueFromDBForEncryptedSummonerID(string encryptedSummonerID, QueueType queueType)
        //{
        //    DataAccessRepository<League> dar = new DataAccessRepository<League>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

        //    League league = dar.FindOne(filter => filter.SummonerID == encryptedSummonerID && filter.QueueType == queueType);

        //    return league;
        //}

        /// <summary>
        /// Get list of historic league snapshots for summoner id given from the db
        /// </summary>
        /// <returns>The list of historic snapshots for given summoner</returns>
        public List<League> GetLeaguesByEncryptedSummonerIDFromDB(string encryptedSummonerID)
        {
            DataAccessRepository<League> dar = new DataAccessRepository<League>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            List<League> leagues = dar.FilterBy(filter => filter.SummonerID == encryptedSummonerID).OrderBy(order => order.CreatedAt).ToList();

            return leagues;
        }

        /// <summary>
        /// Returns the Tier from RiotAPI as an enum
        /// </summary>
        /// <returns>Tier as an enum</returns>
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

        /// <summary>
        /// Returns the QueueType from RiotAPI as an enum
        /// </summary>
        /// <returns>QueueType as an enum</returns>
        private QueueType QueueTypeStringToEnum(string queueType)
        {
            switch (queueType)
            {
                case "RANKED_SOLO_5x5":
                    return QueueType.Ranked_Solo_5x5;
                case "RANKED_FLEX_SR":
                    return QueueType.Ranked_Flex_SR;
                default:
                    return QueueType.Unknown;
            }
        }

        /// <summary>
        /// Returns the Rank from RiotAPI as an enum
        /// </summary>
        /// <returns>Rank as an enum</returns>
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
