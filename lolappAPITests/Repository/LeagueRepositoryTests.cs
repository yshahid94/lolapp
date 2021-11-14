using Microsoft.VisualStudio.TestTools.UnitTesting;
using lolappAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using lolappAPI.Types;

namespace lolappAPI.Repository.Tests
{
    [TestClass()]
    public class LeagueRepositoryTests
    {
        private IServiceCollection _services;
        private IConfiguration _config;

        public LeagueRepositoryTests()
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
        [TestMethod()]
        public void GetLeaguesByEncryptedSummonerIDTest()
        {
            LeagueRepository repository = new LeagueRepository(_config);
            List<League> leagues = repository.GetLeaguesByEncryptedSummonerIDFromRiotAndSaveToDB("wvLyc4f7LWXtOkBg-6KFKhLNNbLMtuXkUufq6buVK-B6U6o");
            Assert.Fail();
        }
    }
}