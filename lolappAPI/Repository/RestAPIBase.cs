using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace lolappAPI.Repository
{
    public abstract class RestAPIBase : IRestAPI
    {
        /// <summary>
        /// The type into which the successful response will deserialise. This will be used in DeserialiseResponse.
        /// </summary>
        public Type ResponseType { get; set; }

        /// <summary>
        /// POST or GET. This will be used in DeserialiseResponse
        /// </summary>
        public RestAPIHelper.MessageType_Enum MessageType { get; set; }

        protected RestTemplate Template { get; set; }

        /// <summary>
        /// Initialise the template here
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Deserialise the error response in whatever format the messages come in
        /// </summary>
        /// <returns></returns>
        public abstract object DeserialiseError(string response, System.Net.HttpStatusCode httpStatusCode, string errorMessage);

        /// <summary>
        /// Deserialise the successful response in whatever format the messages come in
        /// </summary>
        /// <returns></returns>
        public abstract object DeserialiseResponse(string response);

        /// <summary>
        /// This only refers to the end part of the URL, not the base address. 
        /// </summary>
        public string OrderURL { get; set; }

        /// <summary>
        /// The full order URL - A concatenation of URLBase and OrderURL
        /// </summary>
        private string FullURL
        {
            get
            {
                //If either value is not set, throw an exception to say there's an issue with setup
                if (Template.URLBase == null
                    || OrderURL == null)
                {
                    throw new Exception("Both URLBase and OrderURL must be set. Check the template setup");
                }

                string _urlBase = Template.URLBase.EndsWith("/") ? Template.URLBase.TrimEnd('/') : Template.URLBase;
                string _orderURL = this.OrderURL.StartsWith("/") ? this.OrderURL.TrimStart('/') : this.OrderURL;

                return String.Format("{0}/{1}", _urlBase, _orderURL);
            }
        }

        public string RawResponse { get; set; }
        public string Error { get; set; }

        public RestAPIBase()
        {
            Setup();
        }

        public T Post<T>(object request)
        {
            //Set the message type - needed for deserialisation of responses.
            MessageType = RestAPIHelper.MessageType_Enum.POST;

            T response = default(T);

            return response;
        }

        public T Get<T>(string parameters)
        {
            Get_PreValidation();

            //Set the message type - needed for deserialisation of responses.
            MessageType = RestAPIHelper.MessageType_Enum.GET;

            T response = default(T);

            //Reset before new call
            Error = null;

            //Ensure parameters start with a '?' character. If null or empty, leave that way
            parameters = parameters.StartsWith("?") || String.IsNullOrEmpty(parameters) ? parameters : String.Format("?{0}", parameters);

            using (var client = new HttpClient())
            {
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", FullURL, parameters ?? ""));
                client.BaseAddress = new Uri(Template.URLBase);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Template.ContentType));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (Template.Headers != null && Template.Headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> header in Template.Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                Task.Run(async () => {

                    HttpResponseMessage Res = await client.GetAsync(OrderURL);
                    //Storing the response details recieved from web api
                    var rawResponse = Res.Content.ReadAsStringAsync().Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        Object item = JsonObject.Parse(rawResponse);
                        //If response is an array then add it to an object with the property name "array"
                        if (item.GetType().Name == "JsonArray")
                        {
                            JsonObject json = new JsonObject();
                            json.Add(new KeyValuePair<string, JsonNode?>("array", (JsonArray)item));
                            rawResponse = json.ToString();
                        }
                        //Deserialize to object
                        response = (T)DeserialiseResponse(rawResponse);
                    }
                    else
                    {
                        //Handle errors
                        response = (T)DeserialiseError(rawResponse, Res.StatusCode, Res.ReasonPhrase);
                    }

                }).GetAwaiter().GetResult();
            }

            //Reset the params to make sure that this is properly updated for the next call
            ResetParams();
            return response;
        }

        public T DeserializeJSON<T>(string rawResponse)
        {
            T obj;

            //e.g. see MapInServiceOverride()
            obj = JsonConvert.DeserializeObject<T>(rawResponse);

            return obj;
        }

        /// <summary>
        /// Reset properties to ensure they get set properly for the next call
        /// </summary>
        private void ResetParams()
        {
            OrderURL = null;
            MessageType = RestAPIHelper.MessageType_Enum.NotSet;
            ResponseType = null;
        }

        /// <summary>
        /// Returns the value for request headers
        /// </summary>
        /// <param name="HeaderName">The name of the searched header name</param>
        /// <returns></returns>
        public string CheckHeaderValue(string headerName)
        {
            return Template.Headers.SingleOrDefault(kvp => kvp.Key == headerName).Value;
        }

        /// <summary>
        /// Inserts a new header name and value. If the given header name already exists, the current value is replaced
        /// </summary>
        /// <param name="headerName">The header name to be added</param>
        /// <param name="headerValue">The header value to be </param>
        /// <returns>Returns false if a given header name is null or empty</returns>
        public bool InsertHeader(string headerName, string headerValue)
        {
            if (!String.IsNullOrWhiteSpace(headerName))
            {
                int i = Template.Headers.FindIndex(kvp => kvp.Key == headerName);
                if (i == -1) //not found
                {
                    Template.Headers.Add(new KeyValuePair<string, string>(headerName, headerValue));
                }
                else
                {
                    Template.Headers[i] = new KeyValuePair<string, string>(headerName, headerValue);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Pre-validation required to perform the GET request.
        /// </summary>
        public void Get_PreValidation()
        {
            if (ResponseType == null)
            {
                throw new Exception("ResponseType not set. Make sure it is passed into the API's GET method.");
            }
        }
    }
}
