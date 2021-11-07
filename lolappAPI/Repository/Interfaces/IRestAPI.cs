using lolappAPI.Types;

namespace lolappAPI.Repository.Interfaces
{
    public interface IRestAPI
    {
        /// <summary>
        /// The type into which the successful response will deserialise. This will be used in DeserialiseResponse.
        /// </summary>
        Type ResponseType { get; set; }

        /// <summary>
        /// POST or GET. This will be used in DeserialiseResponse
        /// </summary>
        RestAPIHelper.MessageType_Enum MessageType { get; set; }

        void Setup();
        object DeserialiseError(string response, string errorMessage = "");
        object DeserialiseResponse(string response);
    }
}
