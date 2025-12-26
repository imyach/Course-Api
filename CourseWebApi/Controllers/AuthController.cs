
using Application.Common.Commands.Auth.Login;
using Application.Common.Commands.Auth.Logout;
using Application.Common.Commands.Auth.Refresh;
using Application.Common.Commands.Auth.Registration;
using AutoMapper;
using CourseWebApi.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IMapper mapper) : BaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var command = mapper.Map<LoginUserCommand>(loginDto);

            var response = await Mediator.Send(command);
            if (response is null) 
                return Unauthorized();

            return Ok(new {accessToken = response.AccessToken, refreshToken = response.RefreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            var command = mapper.Map<RegistrationUserCommand>(registrationDto);

            var response = await Mediator.Send(command);
            if (response is null)
                return Unauthorized();

            return Ok(new { accessToken = response.AccessToken, refreshToken = response.RefreshToken });

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
        {
            var command = mapper.Map<RefreshTokenCommand>(refreshDto);
            command.CurrentUserId = UserId;

            var response = await Mediator.Send(command);
            if (response is null)
                return Unauthorized();

            return Ok(new { accessToken = response.AccessToken, refreshToken = response.RefreshToken });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutUserCommand
            {
                CurrentUserId = UserId
            };

            var response = await Mediator.Send(command);
            if (response is null)
                return BadRequest();

            return NoContent();
        }
    }
}
