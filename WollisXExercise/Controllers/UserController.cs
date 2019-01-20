using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WollisXExercise.Models;

namespace WollisXExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public ActionResult<User> GetUser()
        {
            return new User
            {
                Name = "Azin Ramezani",
                Token = _configuration[$"{AppSettings.AuthToken}"]
            };
        }
    }
}
