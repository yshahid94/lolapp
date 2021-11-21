using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ApiResponse<Summoner>> GetSummoner(string username)
        {
            var response = await _summonerRepository.GetSummonerAndUpdateIfNeeded(username);
            return new ApiResponse<Summoner>((int)HttpStatusCode.OK, "Success", 1, response);
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
