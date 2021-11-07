namespace lolappAPI.Types
{
    public class RestTemplate
    {
        public RestTemplate()
        {
            Headers = new List<KeyValuePair<string, string>>();
        }

        public string ContentType { get; set; }
        public bool ApplyJSONSerialisation { get; set; }    //NOTE: Maybe we want to get rid of this and have it determine the serialisation based off ContentType? If other forms of serialisation come into play.
        public string URLBase { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}
