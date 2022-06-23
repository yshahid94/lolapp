using lolappAPI.Types;
using Newtonsoft.Json;

namespace lolappAPI.Repository
{
    public class RiotRestAPI : RestAPIBase
    {
        public RiotRestAPI(IConfiguration config): base(config)
        {
        }
        public override void Setup(IConfiguration config)
        {
            RiotAPISettings riotAPISettings = _config.GetSection("RiotAPISettings").Get<RiotAPISettings>();
            Template = new RestTemplate()
            {
                ContentType = "application/json",
                ApplyJSONSerialisation = false,
                URLBase = riotAPISettings.URLBase,
                Headers = new List<KeyValuePair<string, string>>()
            };

            this.InsertHeader("X-Riot-Token", riotAPISettings.APIToken);
        }

        public override object DeserialiseError(string response, System.Net.HttpStatusCode httpStatusCode, string errorMessage)
        {
            RiotInboundMessage deserialisedObject = null;

            deserialisedObject = DeserializeJSON<ErrorMessage>(response);
            ((ErrorMessage)deserialisedObject).Status.StatusCode = httpStatusCode;
            return deserialisedObject;
        }

        public T Post<T>(object request, string orderURL, string urlBase, Type responseType)
        {
            OrderURL = orderURL;
            MessageType = RestAPIHelper.MessageType_Enum.POST;
            ResponseType = responseType;
            Template.URLBase = urlBase;

            return Post<T>(request);
        }

        public T Get<T>(string parameters, string orderURL, string urlBase, Type messageType)
        {
            OrderURL = orderURL;
            ResponseType = messageType;
            Template.URLBase = urlBase;

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
