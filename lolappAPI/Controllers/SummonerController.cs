using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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

        [HttpGet("getAllSummoners")]
        public async Task<ApiResponse<List<Summoner>>> GetAllSummoners()
        {
            var response = await _summonerRepository.GetAllSummonersFromDB();
            return new ApiResponse<List<Summoner>>((int)HttpStatusCode.OK, "Success", response.Count, response);
        }

        [HttpGet("getAllSummonersLeagues")]
        public async Task<ApiResponse<List<SummonerLeagues>>> GetAllSummonersLeagues()
        {
            var response = await _summonerRepository.GetAllSummonersLeagues();
            return new ApiResponse<List<SummonerLeagues>>((int)HttpStatusCode.OK, "Success", response.Count, response);
        }

        [HttpGet("updateAllSummonersLeagues")]
        public async Task<ApiResponse<List<SummonerLeagues>>> UpdateAllSummonersLeagues(bool returnHistoricLeagues = false)
        {
            var response = await _summonerRepository.UpdateAllSummonersLeagues(returnHistoricLeagues);
            return new ApiResponse<List<SummonerLeagues>>((int)HttpStatusCode.OK, "Success", response.Count, response);
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
