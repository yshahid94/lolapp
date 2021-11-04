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

namespace lolappAPI.Repository.Tests
{
    [TestClass()]
    public class DataAccessRepositoryTests
    {
        private IConfiguration _config;
        private IServiceCollection _services;

        public DataAccessRepositoryTests()
        {
            _services = new ServiceCollection();

            _services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            _services.AddSingleton<IConfiguration>(Configuration);

        }
        //[TestInitialize]
        //public void SetUp()
        //{
        //    _services = new ServiceCollection();

        //    _services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
        //    _services.AddSingleton<IConfiguration>(Configuration);
        //}

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
            DataAccessRepository<User> dar = new DataAccessRepository<User>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            //Create document
            var reqUser = new User() { Username = "testUser", Rank = "gold" };
            //Insert
            dar.InsertOne(reqUser);
            //Find it
            User resUser = dar.FindOne(filter => filter.Username == "testUser" && filter.Id == reqUser.Id);
            //Asserts
            Assert.IsTrue(resUser.Id == reqUser.Id);
            //Clean up
            dar.DeleteById(resUser.Id);
        }

        [TestMethod("Insert Row")]
        public void InsertRows()
        {
           DataAccessRepository<User> dar = new DataAccessRepository<User>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());

            //Create document
            var reqUser = new User() { Username = "testUser", Rank = "gold" };
            //Insert
            dar.InsertOne(reqUser);
            //Find it
            User resUser = dar.FindOne(filter => filter.Username == "testUser" && filter.Id == reqUser.Id);
            //Asserts
            Assert.IsTrue(resUser.Id == reqUser.Id);
            //Clean up
            dar.DeleteById(resUser.Id);
        }

        [TestMethod("Update Row")]
        public void UpdateRows()
        {
            DataAccessRepository<User> dar = new DataAccessRepository<User>(_config.GetSection("MongoDbSettings").Get<MongoDbSettings>());
            var PLAT = "plat";

            //Create document
            var reqUser = new User() { Username = "testUser", Rank = "gold" };
            //Insert
            dar.InsertOne(reqUser);
            //Update
            reqUser.Rank = PLAT;
            dar.ReplaceOne(reqUser);
            //Find it
            User resUser = dar.FindOne(filter => filter.Rank == PLAT && filter.Id == reqUser.Id);
            //Asserts
            Assert.IsTrue(resUser.Id == reqUser.Id && resUser.Rank == PLAT);
            //Clean up
            dar.DeleteById(resUser.Id);
        }
    }
}