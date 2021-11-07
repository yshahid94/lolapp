using lolappAPI.Types;

namespace lolappAPI.Repository.Interfaces
{
    public interface ISummonerRepository
    {
        public Summoner GetSummonerAndUpdateIfNeeded(string Name, bool forceUpdate = false);
    }
}
