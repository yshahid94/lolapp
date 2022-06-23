using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using lolappAPI.Types;
using Newtonsoft.Json;
using lolappAPI.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace lolappAPITests.Repository
{
    [TestClass()]
    public class RiotAPIRepositoryTests
    {

        private IServiceCollection _services;
        private IConfiguration _config;
        private RiotAPISettings _riotAPISettings;

        public RiotAPIRepositoryTests()
        {
            _services = new ServiceCollection();

            _services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            _services.AddSingleton<IConfiguration>(Configuration);

            _riotAPISettings = _config.GetSection("RiotAPISettings").Get<RiotAPISettings>();
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

        [TestMethod("ss")]
        public void aa()
        {
            var EmpResponse = "";
            GetSummonerInboundMessage summoner = new GetSummonerInboundMessage();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(_riotAPISettings.URLBase);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Riot-Token", _riotAPISettings.APIToken);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                //HttpResponseMessage Res = await client.GetAsync("lol/summoner/v4/summoners/by-name/Yassin");
                Task.Run(async () =>
                {



                    HttpResponseMessage Res = await client.GetAsync("lol/summoner/v4/summoners/by-name/Yassin");
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        //EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                        summoner = JsonConvert.DeserializeObject<GetSummonerInboundMessage>(EmpResponse);
                    }


                }).GetAwaiter().GetResult();
                //AsyncContext.Run(() => GetAsync(id));
                //Checking the response is successful or not which is sent using HttpClient
                //
            }

        }


        [TestMethod("2")]
        public void qwdd()
        {
            List<RiotInboundMessage> responses = new List<RiotInboundMessage>();

            GetSummonerByNameOutboundMessage req = new GetSummonerByNameOutboundMessage();
            req.Name = "Yassin";

            var restAPI = new RiotRestAPI(_config);

            var response = RiotRestAPI.SendMessage(req, restAPI);
        }
    }
}
