using Microsoft.AspNetCore.Mvc;
using Messenger.Interfaces;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private readonly IUserRepository _userRepository;

        public DataController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Seeds the database with demo data.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /data/seed
        ///
        /// </remarks>
        /// <response code="200">Returns confirmation of seeded users if successful or information to clear the database if data exists.</response>
        [HttpGet]
        [Route("Seed")]
        public string SeedData()
        {
            return _userRepository.SeedData();
        }

        /// <summary>
        /// Clears the data in the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /data/clear
        ///
        /// </remarks>
        /// <response code="200">Confirms if data is cleared.</response>
        [HttpGet]
        [Route("Clear")]
        public string ClearData()
        {
            return _userRepository.ClearData();
        }
    }
}
