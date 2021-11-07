using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;

namespace lolappAPI.Repository
{
    public class SummonerRepository : ISummonerRepository
    {
        private IConfiguration _configuration;
        public SummonerRepository(IConfiguration config)
        {
            _configuration = config;
        }
        private Summoner GetSummonerByNameFromRiot(string name)
        {
            List<RiotInboundMessage> responses = new List<RiotInboundMessage>();

            GetSummonerByNameOutboundMessage req = new GetSummonerByNameOutboundMessage();
            req.Name = name;

            var restAPI = new RiotRestAPI();

            GetSummonerInboundMessage response = (GetSummonerInboundMessage)RiotRestAPI.SendMessage(req, restAPI);

            Summoner summoner = new Summoner();

            summoner.UserID = response.ID;
            summoner.AccountID = response.AccountID;
            summoner.PUUID = response.PUUID;
            summoner.Name = response.Name;
            summoner.ProfileIconID = response.ProfileIconID;
            summoner.RevisionDate = response.RevisionDate;
            summoner.SummonerLevel = response.SummonerLevel;

            return summoner;
        }
        private Summoner GetSummonerByNameFromDB(string name)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            Summoner resUser = dar.FindOne(filter => filter.Name == name);

            return resUser;
        }
        private Summoner InsertSummonerToDB(Summoner summoner)
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            dar.InsertOne(summoner);

            //Summoner resUser = dar.FindOne(filter => filter.Id == summoner.Id);

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
