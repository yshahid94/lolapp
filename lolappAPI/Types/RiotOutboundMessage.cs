using System.Text;

namespace lolappAPI.Types
{
    public abstract class RiotOutboundMessage
    {
        public enum MessageType_Enum
        {
            POST,
            GET
        }
        public void CreateMessage(MessageType_Enum messageType, Type responseType, RiotAPIURLBaseVersion version)
        {
            this.Params = new List<KeyValuePair<string, string>>();
            this._MessageType = messageType;
            this._ResponseType = responseType;
        }

        public void CreateMessage(MessageType_Enum messageType, Type responseType, RiotAPIURLBaseVersion version, object body)
        {
            this.Params = new List<KeyValuePair<string, string>>();
            this._MessageType = messageType;
            this._ResponseType = responseType;
            this._Body = body;
            this.RiotAPIURLBaseVersion = version;
        }

        public void CreateMessage(MessageType_Enum messageType, Type responseType, RiotAPIURLBaseVersion version, object body, List<KeyValuePair<string, string>> paras)
        {
            this.Params = paras;
            this._MessageType = messageType;
            this._ResponseType = responseType;
            this._Body = body;
            this.RiotAPIURLBaseVersion = version;
        }

        public Type ResponseType { get { return _ResponseType; } }
        public MessageType_Enum MessageType { get { return _MessageType; } }
        public object Body { get { return _Body; } }

        private object _Body { get; set; }
        private MessageType_Enum _MessageType { get; set; }
        private Type _ResponseType { get; set; }

        /// <summary>
        /// This only refers to the end part of the URL, not the base address. 
        /// E.g. https://api-test.gamma.co.uk/horizon-provisioning/v1/companies/{companyId} -> companies/{companyId}
        /// </summary>
        public abstract string OrderURL { get; }
        public string URLBase { get { return RiotAPIURLBaseVersion == RiotAPIURLBaseVersion.Old ? "https://euw1.api.riotgames.com/" : "https://europe.api.riotgames.com/"; } }
        private RiotAPIURLBaseVersion RiotAPIURLBaseVersion;

        private List<KeyValuePair<string, string>> Params { get; set; }

        //TODO - Maybe move this to RestAPI?
        /// <summary>
        /// A concatenated string build from the Params property
        /// </summary>
        public string ParamString
        {
            //TODO - Untested. Once tested, remove this.
            get
            {
                StringBuilder sb = new StringBuilder();

                if (this.Params.Count > 0)
                {
                    sb.Append("?");
                }

                foreach (KeyValuePair<string, string> param in this.Params)
                {
                    sb.Append(param.Key);
                    sb.Append("=");
                    sb.Append(param.Value);
                    sb.Append("&");
                }

                //Remove the final &
                if (this.Params.Count > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                return sb.ToString();
            }
        }
    }
}
