
using Application.Common.Commands.Auth.Login;
using AutoMapper;
using CourseWebApi.Models.Auth;
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
            if (response == string.Empty) 
                return Unauthorized();

            return Ok(new {token = response});

        }
    }
}
