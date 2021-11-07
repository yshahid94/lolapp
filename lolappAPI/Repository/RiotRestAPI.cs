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
            //var obj = new Zen_InboundMessage();

            //if (response.Contains("Availability has expired"))
            //{
            //    obj = DeserializeJSON<ZenError_InboundMessage>(response);
            //}
            //else if (response.Contains("Could not find availability details"))
            //{
            //    obj = DeserializeJSON<ZenError_InboundMessage>(response);
            //}
            //else
            //{
            //    obj = new ZenError_InboundMessage()
            //    {
            //        errors = new List<ZenError>() { new ZenError() { message = response } }.ToArray()
            //    };
            //}

            return obj;
        }

        /// <summary>
        /// Create a POST request to Zen
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a GET request to Zen
        /// </summary>
        /// <param name="parameters">The parameters to append onto the query string</param>
        /// <param name="orderURL">The order's URL</param>
        /// <param name="responseType">The type into which the successful response will deserialise. This will be used in DeserialiseResponse.</param>
        /// <returns></returns>
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
                string zenRequestJSON = JsonConvert.SerializeObject(message.Body);
                response = restAPI.Post<RiotInboundMessage>(zenRequestJSON, message.OrderURL, message.URLBase, message.ResponseType);
            }

            return response;
        }
    }
}
