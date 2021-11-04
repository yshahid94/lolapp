using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace lolappAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDataAccessRepository<User> _userRepository;

        public UserController(IDataAccessRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("registerPerson")]
        public async Task AddUser(string firstName, string lastName)
        {
            var person = new User()
            {
                Username = firstName,
                Rank = lastName
            };

            await _userRepository.InsertOneAsync(person);
        }

        [HttpGet("getPeopleData")]
        public IEnumerable<string> GetPeopleData()
        {
            var people = _userRepository.FilterBy(
                filter => filter.Username != "test",
                projection => projection.Username
            );
            return people.ToList();
        }
    }
}
