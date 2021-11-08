using lolappAPI.Types;
using Newtonsoft.Json;

namespace lolappAPI.Repository
{
    public class RiotRestAPI : RestAPIBase
    {
        public override void Setup()
        {
            Template = new RestTemplate()
            {
                ContentType = "application/json",
                ApplyJSONSerialisation = false,
                URLBase = "https://euw1.api.riotgames.com/",
                Headers = new List<KeyValuePair<string, string>>()
            };
        }

        public override object DeserialiseError(string response, string errorMessage)
        {
            var obj = response;

            return obj;
        }

        public T Post<T>(object request, string orderURL, string urlBase, Type responseType)
        {
            OrderURL = orderURL;
            MessageType = RestAPIHelper.MessageType_Enum.POST;
            ResponseType = responseType;
            Template.URLBase = urlBase;

            //Need to generate token here
            this.InsertHeader("X-Riot-Token", "RGAPI-fedc71a0-f211-4117-b97c-6d8c4398a68a");

            return Post<T>(request);
        }

        public T Get<T>(string parameters, string orderURL, string urlBase, Type messageType)
        {
            OrderURL = orderURL;
            ResponseType = messageType;
            Template.URLBase = urlBase;

            //Need to generate token here
            this.InsertHeader("X-Riot-Token", "RGAPI-fedc71a0-f211-4117-b97c-6d8c4398a68a");

            return Get<T>(parameters);
        }

        public override object DeserialiseResponse(string response)
        {
            RiotInboundMessage deserialisedObject = null;

            if (ResponseType.FullName == typeof(GetSummonerInboundMessage).FullName)
            {
                deserialisedObject = DeserializeJSON<GetSummonerInboundMessage>(response);
            }
            else if (ResponseType.FullName == typeof(GetLeagueBySummonerInboundMessage).FullName)
            {
                deserialisedObject = DeserializeJSON<GetLeagueBySummonerInboundMessage>(response);
            }
            return deserialisedObject;
        }
        public static RiotInboundMessage SendMessage(RiotOutboundMessage message, RiotRestAPI restAPI)
        {
            RiotInboundMessage response = null;

            if (message.MessageType == RiotOutboundMessage.MessageType_Enum.GET)
            {
                response = restAPI.Get<RiotInboundMessage>(message.ParamString, message.OrderURL, message.URLBase, message.ResponseType);
            }
            else if (message.MessageType == RiotOutboundMessage.MessageType_Enum.POST)
            {
                string riotRequestJSON = JsonConvert.SerializeObject(message.Body);
                response = restAPI.Post<RiotInboundMessage>(riotRequestJSON, message.OrderURL, message.URLBase, message.ResponseType);
            }

            return response;
        }
    }
}
