using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace lolappAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueController(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        //[HttpGet("getSummoner")]
        //public async Task<ApiResponse<Summoner>> UpdateAllSummonerLeagues()
        //{
        //    var response = await _leagueRepository.GetSummonerAndUpdateIfNeeded(username);
        //    return new ApiResponse<Summoner>((int)HttpStatusCode.OK, "Success", 1, response);
        //}

        //[HttpGet("getSummoner")]
        //public async Task<ApiResponse<Summoner>> GetAllSummonerLeagues()
        //{
        //    var response = await _leagueRepository.GetSummonerAndUpdateIfNeeded(username);
        //    return new ApiResponse<Summoner>((int)HttpStatusCode.OK, "Success", 1, response);
        //}
    }
}
