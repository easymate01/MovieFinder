using Microsoft.AspNetCore.Mvc;
using MovieFInderServer.Models;
using MovieFInderServer.Services.Users;

namespace MovieFInderServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create/user")]
        public async Task<ActionResult> CreateUser(User user)
        {
            var existingUser = await _userService.GetUserByIdAsync(user.UserId);
            if (existingUser != null)
            {
                return Conflict("User already exists");
            }

            await _userService.AddUser(user);
            return Ok("User created.");
        }
    }
}
