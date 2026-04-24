
using Application.Common.Commands.Auth.Login;
using Application.Common.Commands.Auth.Logout;
using Application.Common.Commands.Auth.Refresh;
using Application.Common.Commands.Auth.Registration;
using Application.Common.Commands.Auth.SendingCodeEmail;
using Application.Common.Commands.Auth.SendingPasswordEmail;
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
        /// <summary>
        /// Account authorization
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/Auth/login
        /// {
        ///   "login": "string",
        ///   "password": "string"
        /// }
        /// </remarks>
        /// <param name="loginDto">LoginDto object</param>
        /// <returns>Returns access and refresh tokens</returns>
        /// <response code="200">Siccess</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var command = mapper.Map<LoginUserCommand>(loginDto);

            var response = await Mediator.Send(command);
            if (response is null) 
                return Unauthorized();

            return Ok(new {accessToken = response.AccessToken, refreshToken = response.RefreshToken });
        }

        /// <summary>
        /// Account registration
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/Auth/register
        ///{
        ///  "nameUser": "string",
        ///  "login": "string",
        ///  "password": "string",
        ///  "email": "string",
        ///  "phoneNumber": "string"
        ///}
        /// </remarks>
        /// <param name="registrationDto">RegistrationDto object</param>
        /// <returns>Returns access and refresh tokens</returns>
        /// <response code="200">Siccess</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            var command = mapper.Map<RegistrationUserCommand>(registrationDto);

            var response = await Mediator.Send(command);
            if (response is null)
                return Unauthorized();

            return Ok(new { accessToken = response.AccessToken, refreshToken = response.RefreshToken });

        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/Auth/refresh
        ///{
        ///  "refreshToken": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///}
        /// </remarks>
        /// <param name="refreshDto">RefreshDto object</param>
        /// <returns>Returns access and refresh tokens</returns>
        /// <response code="200">Siccess</response>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
        {
            var command = mapper.Map<RefreshTokenCommand>(refreshDto);

            var response = await Mediator.Send(command);
            if (response is null)
                return Unauthorized();

            return Ok(new { accessToken = response.AccessToken, refreshToken = response.RefreshToken });
        }
        /// <summary>
        /// Log out of the user account
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/Auth/logout
        /// </remarks>
        /// <param>Without params</param>
        /// <returns>Returns access and refresh tokens</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Send recovery code by email
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// POST (HOST)/api/Auth/sendRecoveryCode?userEmail=email@mail.com&userId=D8F05718-1FD2-43A2-B309-CD71DDD02005
        /// </remarks>
        /// <param name="userEmail">user email for send message with recovery code</param>
        /// <param name="userId">user id</param>
        /// <response code="200">Siccess</response>
        [HttpPost("sendRecoveryCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendRecoveryCodeByEmail([FromQuery] string userEmail, [FromQuery] Guid userId)
        {
            var command = new SendingTheCodeByEmailCommand
            {
                UserEmail = userEmail,
                UserId = userId 
            };
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Send new password for user
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// POST (HOST)/api/Auth/sendNewPassword?code=543634&userId=D8F05718-1FD2-43A2-B309-CD71DDD02005
        /// </remarks>
        /// <param name="code">user recovery code</param>
        /// <param name="userId">user id</param>
        /// <response code="200">Siccess</response>
        [HttpPost("sendNewPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> SendNewPasswordByEmail([FromQuery] string code, [FromQuery] Guid userId)
        {
            var command = new SendingThePasswordByEmailCommand
            {
                Code = code,
                UserId = userId
            };
            var responce = await Mediator.Send(command);
            return Ok(responce);
        }
    }
}
