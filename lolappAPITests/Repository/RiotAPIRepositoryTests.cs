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

namespace lolappAPITests.Repository
{
    [TestClass()]
    public class RiotAPIRepositoryTests
    {
        private const string URL = "https://euw1.api.riotgames.com/";
        private string urlParameters = "?api_key=123";

        [TestMethod("ss")]
        public void aa()
        {
            var EmpResponse = "";
            GetSummonerInboundMessage summoner = new GetSummonerInboundMessage();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri("https://euw1.api.riotgames.com/");
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Riot-Token", "RGAPI-9daa7b6e-d732-4f7f-9c54-8a1dd3b020ad");
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

            var restAPI = new RiotRestAPI();

            var response = RiotRestAPI.SendMessage(req, restAPI);
        }
    }
}
