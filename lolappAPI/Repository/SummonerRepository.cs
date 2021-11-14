using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;

namespace lolappAPI.Repository
{
    public class SummonerRepository : ISummonerRepository
    {
        private IConfiguration _configuration;
        RiotRestAPI _restAPI = null;
        LeagueRepository _leagueRepository = null;
        public SummonerRepository(IConfiguration config)
        {
            _configuration = config;
            _leagueRepository = new LeagueRepository(config);
        }
        public List<SummonerLeagues> GetAllSummonersLeagues()
        {
            //Get all summoners from db
            List<Summoner> allDBSummoners = GetAllSummonersFromDB();
            //Get updated leagues from Riot insert them to db and return them
            List<SummonerLeagues> summonerLeaguesList = allDBSummoners.Select(x => new SummonerLeagues() { Summoner = x, HistoricLeagues = _leagueRepository.GetLeaguesByEncryptedSummonerIDFromDB(x.SummonerID) }).ToList();
            
            return summonerLeaguesList;
        }
        public List<SummonerLeagues> UpdateAllSummonersLeagues(bool returnHistoricLeagues = false)
        {
            //Get all summoners from db
            List<Summoner> allDBSummoners = GetAllSummonersFromDB();
            //Get updated leagues from Riot insert them to db and return them
            List<SummonerLeagues> summonerLeaguesList = allDBSummoners.Select(x => new SummonerLeagues() { Summoner = x, HistoricLeagues = _leagueRepository.GetLeaguesByEncryptedSummonerIDFromRiotAndSaveToDB(x.SummonerID) }).ToList();
            if(returnHistoricLeagues)
            {
                //Get all historic leagues from the db
                summonerLeaguesList = allDBSummoners.Select(x => new SummonerLeagues() { Summoner = x, HistoricLeagues = _leagueRepository.GetLeaguesByEncryptedSummonerIDFromDB(x.SummonerID) }).ToList();
            }
            return summonerLeaguesList;
        }
        private Summoner GetSummonerByNameFromRiot(string name)
        {
            List<RiotInboundMessage> responses = new List<RiotInboundMessage>();

            GetSummonerByNameOutboundMessage req = new GetSummonerByNameOutboundMessage();
            req.Name = name;

            _restAPI = new RiotRestAPI();
            
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

        private Summoner GetSummonerByNameFromDB(string name)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            Summoner summoner = dar.FindOne(filter => filter.Name == name);

            return summoner;
        }
        private List<Summoner> GetAllSummonersFromDB()
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            List<Summoner> summoners = dar.FilterBy(filter => true).ToList();

            return summoners;
        }
        private Summoner InsertSummonerToDB(Summoner summoner)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            dar.InsertOne(summoner);

            return summoner;
        }
        private Summoner UpdateDBSummoner(Summoner summoner)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            dar.ReplaceOne(summoner);

            return summoner;
        }
        public Summoner GetSummonerAndUpdateIfNeeded(string name, bool forceUpdate = false)
        {
            Summoner dbSummoner = GetSummonerByNameFromDB(name);

            if(dbSummoner == null)
            {
                Summoner riotSummoner = GetSummonerByNameFromRiot(name);
                dbSummoner = InsertSummonerToDB(riotSummoner);
            }
            //If last time updated was more than a day ago update
            else if(forceUpdate || dbSummoner.UpdatedOn < DateTime.Now.AddDays(-1))
            {
                Summoner riotSummoner = GetSummonerByNameFromRiot(name);
                dbSummoner.SummonerLevel = riotSummoner.SummonerLevel;
                dbSummoner.RevisionDate = riotSummoner.RevisionDate;
                dbSummoner.ProfileIconID = riotSummoner.ProfileIconID;

                UpdateDBSummoner(dbSummoner);
            }

            return dbSummoner;
        }

        
    }
}
