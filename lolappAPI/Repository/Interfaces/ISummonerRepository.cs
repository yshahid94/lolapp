using lolappAPI.Types;

namespace lolappAPI.Repository.Interfaces
{
    public interface ISummonerRepository
    {
        Task<Summoner> GetSummonerAndUpdateIfNeeded(string Name, bool forceUpdate = false);
        Task<List<Summoner>> GetAllSummonersFromDB();
        Task<List<SummonerLeagues>> GetAllSummonersLeagues();
        Task<List<SummonerLeagues>> UpdateAllSummonersLeagues(bool returnHistoricLeagues = false);
    }
}
