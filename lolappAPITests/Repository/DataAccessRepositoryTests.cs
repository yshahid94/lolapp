using Microsoft.VisualStudio.TestTools.UnitTesting;
using lolappAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Bson;
using lolappAPI.Types;
using System.Net.Http;
using System.Net.Http.Headers;

namespace lolappAPI.Repository.Tests
{
    [TestClass()]
    public class DataAccessRepositoryTests
    {
        private IConfiguration _config = null;
        private IServiceCollection _services = null;

        public DataAccessRepositoryTests()
        {
            _services = new ServiceCollection();

            _services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            _services.AddSingleton<IConfiguration>(Configuration);

        }

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.Development.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        [TestMethod("Find Row")]
        public void FindRows()
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            //Create document
            var reqUser = new Summoner() { Name = "testUser", SummonerLevel = 123 };
            //Insert
            dar.InsertOne(reqUser);
            //Find it
            Summoner resUser = dar.FindOne(filter => filter.Name == "testUser" && filter.Id == reqUser.Id);
            //Asserts
            Assert.IsTrue(resUser.Id == reqUser.Id);
            //Clean up
            dar.DeleteById(resUser.Id);
        }

        [TestMethod("Insert Row")]
        public void InsertRows()
        {
           DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            //Create document
            var reqUser = new Summoner() { Name = "testUser", SummonerLevel = 123 };
            //Insert
            dar.InsertOne(reqUser);
            //Find it
            Summoner resUser = dar.FindOne(filter => filter.Name == "testUser" && filter.Id == reqUser.Id);
            //Asserts
            Assert.IsTrue(resUser.Id == reqUser.Id);
            //Clean up
            dar.DeleteById(resUser.Id);
        }

        [TestMethod("Update Row")]
        public void UpdateRows()
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());
            
            //Create document
            var reqUser = new Summoner() { Name = "testUser", SummonerLevel = 123 };
            //Insert
            dar.InsertOne(reqUser);
            //Update
            reqUser.SummonerLevel = 321;
            dar.ReplaceOne(reqUser);
            //Find it
            Summoner resUser = dar.FindOne(filter => filter.SummonerLevel == 321 && filter.Id == reqUser.Id);
            //Asserts
            Assert.IsTrue(resUser.Id == reqUser.Id && resUser.SummonerLevel == 321);
            //Clean up
            dar.DeleteById(resUser.Id);
        }
        [TestMethod("Get All Rows")]
        public void GetAllRows()
        {
            DataAccessRepository<Summoner> dar = new DataAccessRepository<Summoner>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            //Create document
            var req = new Summoner() { Name = "testUser", SummonerLevel = 123 };
            //Insert
            dar.InsertOne(req);
            //Find all summoners
            List<Summoner> resSummoners = dar.FilterBy(filter => true).ToList();
            //Asserts
            Assert.IsTrue(resSummoners.Count() > 0);
            Assert.IsTrue(resSummoners.Any(x => x.Id == req.Id));
            //Clean up
            dar.DeleteById(req.Id);
        }


    }
}