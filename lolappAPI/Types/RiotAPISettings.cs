using lolappAPI.Types.Interfaces;

namespace lolappAPI.Types
{
    public class RiotAPISettings : IRiotAPISettings
    {
        public string URLBase { get; set; }
        public string APIToken { get; set; }
    }
}
