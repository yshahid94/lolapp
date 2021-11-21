using lolappAPI.Types;

namespace lolappAPI.Repository.Interfaces
{
    public interface ISummonerRepository
    {
        Task<Summoner> GetSummonerAndUpdateIfNeeded(string Name, bool forceUpdate = false);
    }
}
