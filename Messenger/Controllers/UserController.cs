using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger.Models;
using Messenger.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Messenger.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves an existing user from the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /User/{userName}
        ///
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Returns the User</returns>
        /// <response code="200">If the user exists</response>
        /// <response code="409">If a user exists with username</response>  
        [HttpGet("{userName}", Name = "GetUser")]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult Get(string userName)
        {
            var user = _userRepository.GetUserByUserName(userName);
            if(user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        /// <summary>
        /// Creates a user with a given userName, no Id needed.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///     {
        ///        "userName": "vidjon",
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>A newly-created User and the route to the user.</returns>
        /// <response code="201">Returns the newly-created user</response>
        /// <response code="400">If the username is null or empty</response>
        /// <response code="409">If a user exists with username</response>  
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (_userRepository.UserExists(user.UserName))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            
            _userRepository.AddUser(user);

            return CreatedAtRoute("GetUser", new { userName = user.UserName }, user);

        }
    }
}
