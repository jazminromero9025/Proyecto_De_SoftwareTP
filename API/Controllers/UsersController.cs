using Application.Interfaces;
using Application.Models;
using Application.UseCases.Users;
using Application.UseCases.Users.command;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginCommand command)
        {
            var user = await _userService.AuthenticateAsync(command);

            if (user == null)
            {
                return Unauthorized(new { message = "Email o contraseña incorrectos." });
            }

            return Ok(user);




        }
    }
}
