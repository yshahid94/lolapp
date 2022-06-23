using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;

namespace lolappAPI.Repository
{
    public class SummonerRepository : ISummonerRepository
    {
        private IConfiguration _config;
        RiotRestAPI _restAPI = null;
        LeagueRepository _leagueRepository = null;
        public SummonerRepository(IConfiguration config)
        {
            _config = config;
            _leagueRepository = new LeagueRepository(config);
        }
        /// <summary>
        /// Gets the list of historic leagues for summoners under lolapp.summoners.
        /// </summary>
        /// <remarks>
        /// First gets the list of summoners under lolapp.summoners
        /// then grabs all the historic leagues for those summoners and returns them
        /// </remarks>
        /// <returns>List of all historic leagues for all summoners</returns>
        public async Task<List<SummonerLeagues>> GetAllSummonersLeagues()
        {
            //Get all summoners from db
            List<Summoner> allDBSummoners = await GetAllSummonersFromDB();
            //Get updated leagues from Riot insert them to db and return them
            List<SummonerLeagues> summonerLeaguesList = allDBSummoners.Select(x => new SummonerLeagues() { Summoner = x, HistoricLeagues = _leagueRepository.GetLeaguesByEncryptedSummonerIDFromDB(x.SummonerID) }).ToList();
            
            return summonerLeaguesList;
        }
        /// <summary>
        /// Grabs the current league snapshot for all summoners under lolapp.summoners and stores them in db.
        /// </summary>
        /// <remarks>
        /// First gets the list of summoners under lolapp.summoners
        /// gets all of their current league snapshots from RiotAPI
        /// then returns them with or without the historic leagues on the db depending on the param
        /// </remarks>
        /// <returns>List of all historic leagues for all summoners</returns>
        public async Task<List<SummonerLeagues>> UpdateAllSummonersLeagues(bool returnHistoricLeagues = false)
        {
            //Get all summoners from db
            List<Summoner> allDBSummoners = await GetAllSummonersFromDB();
            //Get updated leagues from Riot insert them to db and return them
            List<SummonerLeagues> summonerLeaguesList = allDBSummoners.Select(x => new SummonerLeagues() { Summoner = x, HistoricLeagues = _leagueRepository.GetLeaguesByEncryptedSummonerIDFromRiotAndSaveToDB(x.SummonerID) }).ToList();
            if(returnHistoricLeagues)
            {
                //Get all historic leagues from the db
                summonerLeaguesList = allDBSummoners.Select(x => new SummonerLeagues() { Summoner = x, HistoricLeagues = _leagueRepository.GetLeaguesByEncryptedSummonerIDFromDB(x.SummonerID) }).ToList();
            }
            return summonerLeaguesList;
        }
        /// <summary>
        /// Grabs the summoners 
        /// </summary>
        /// <remarks>
        /// First gets the list of summoners under lolapp.summoners
        /// gets all of their current league snapshots from RiotAPI
        /// then returns them with or without the historic leagues on the db depending on the param
        /// </remarks>
        /// <returns>List of all historic leagues for all summoners</returns>
        private async Task<Summoner> GetSummonerByNameFromRiot(string name)
        {
            List<RiotInboundMessage> responses = new List<RiotInboundMessage>();

            GetSummonerByNameOutboundMessage req = new GetSummonerByNameOutboundMessage();
            req.Name = name;

            _restAPI = new RiotRestAPI(_config);
            
            RiotInboundMessage response = (RiotInboundMessage)RiotRestAPI.SendMessage(req, _restAPI);

            if(response is ErrorMessage)
            {
                throw new Exception(((ErrorMessage)response).Status.Message);
            }

            GetSummonerInboundMessage summonerResponse = (GetSummonerInboundMessage)response;

            Summoner summoner = new Summoner();

            summoner.SummonerID = summonerResponse.ID;
            summoner.AccountID = summonerResponse.AccountID;
            summoner.PUUID = summonerResponse.PUUID;
            summoner.Name = summonerResponse.Name;
            summoner.ProfileIconID = summonerResponse.ProfileIconID;
            summoner.RevisionDate = summonerResponse.RevisionDate;
            summoner.SummonerLevel = summonerResponse.SummonerLevel;

            return summoner;
        }

        private async Task<Summoner> GetSummonerByNameFromDB(string name)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            Summoner summoner = await dar.FindOneAsync(filter => filter.Name == name);

            return summoner;
        }
        public async Task<List<Summoner>> GetAllSummonersFromDB()
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            IEnumerable<Summoner> summoners = await dar.FilterByAsync(filter => true);

            return summoners.ToList();
        }
        private async Task<Summoner> InsertSummonerToDB(Summoner summoner)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            await dar.InsertOneAsync(summoner);

            return summoner;
        }
        private async Task<Summoner> UpdateDBSummoner(Summoner summoner)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            dar.ReplaceOne(summoner);

            return summoner;
        }
        public async Task<Summoner> GetSummonerAndUpdateIfNeeded(string name, bool forceUpdate = false)
        {
            Summoner dbSummoner = await GetSummonerByNameFromDB(name);

            if(dbSummoner == null)
            {
                Summoner riotSummoner = await GetSummonerByNameFromRiot(name);
                dbSummoner = await InsertSummonerToDB(riotSummoner);
            }
            //If last time updated was more than a day ago update
            else if(forceUpdate || dbSummoner.UpdatedOn < DateTime.Now.AddDays(-1))
            {
                Summoner riotSummoner = await GetSummonerByNameFromRiot(name);
                dbSummoner.SummonerLevel = riotSummoner.SummonerLevel;
                dbSummoner.RevisionDate = riotSummoner.RevisionDate;
                dbSummoner.ProfileIconID = riotSummoner.ProfileIconID;

                await UpdateDBSummoner(dbSummoner);
            }

            return dbSummoner;
        }

        
    }
}
