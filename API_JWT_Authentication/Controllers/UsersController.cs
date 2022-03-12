using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_JWT_Authentication.Models;
using API_JWT_Authentication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLogin userLogin)
        {
            var user = _userRepo.Authenticate(userLogin.Username, userLogin.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserLogin userLogin)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(userLogin.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = _userRepo.Register(userLogin.Username, userLogin.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }
    }
}