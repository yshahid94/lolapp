using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace lolappAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummonerController : ControllerBase
    {
        private readonly ISummonerRepository _summonerRepository;

        public SummonerController(ISummonerRepository summonerRepository)
        {
            _summonerRepository = summonerRepository;
        }

        [HttpGet("getSummoner")]
        public Summoner GetSummoner(string username)
        {
            return _summonerRepository.GetSummonerAndUpdateIfNeeded(username);
        }

        //[HttpGet("getPeopleData")]
        //public IEnumerable<string> GetPeopleData()
        //{
        //    var people = _summonerRepository.FilterBy(
        //        filter => filter.Name != "test",
        //        projection => projection.Name
        //    );
        //    return people.ToList();
        //}
    }
}
