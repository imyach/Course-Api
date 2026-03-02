using Application.Common.Commands.Users.CreateUser;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Commands.Users.UpdateUserForAdmin;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Users.GetUser;
using Application.Common.Queries.Users.GetUsersList;
using AutoMapper;
using CourseWebApi.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/user/all
        /// </remarks>
        /// <param name="pageNumber">Page Number </param>
        /// <param name="pageSize">The number of items returned by the server</param>
        /// <param name="searchText">Text to search for items</param>
        /// <returns>Returns UsersListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UsersListVm>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchText = null)
        {
            var query = new GetAllUsersQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchText = searchText
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/user/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">User id guid</param>
        /// <returns>Returns UserLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsUserQuery()
            {
                Id = id,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Create object user 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/user
        /// {
        ///     "nameUser": "string",
        ///     "login": "string",
        ///     "password": "string",
        ///     "email": "string",
        ///     "phoneNumber": "string",
        ///     "role": "object"
        /// }
        /// </remarks>
        /// <param name="createUserCommand">CreateUserDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserDto createUserCommand)
        {
            var command = mapper.Map<CreateUserCommand>(createUserCommand);
            command.CurrentUserId = UserId;
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }

        /// <summary>
        /// Delete object user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/user/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">User id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand()
            {
                CurrentUserId = UserId,
                Id = id,
            };
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Update object user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/user
        /// {
        ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "nameUser": "string",
        ///     "login": "string",
        ///     "password": "string",
        ///     "email": "string",
        ///     "phoneNumber": "string",
        ///     "role": "object"
        /// }
        /// </remarks>
        /// <param name="updateUserCommand">UpdateUserDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserCommand)
        {
            var command = mapper.Map<UpdateUserCommand>(updateUserCommand);
            command.CurrentUserId = UserId;

            var response = await Mediator.Send(command);

            if (response is null)
                return NoContent();

            return Ok(new {accessToken = response.AccessToken, refreshToken = response.RefreshToken});
        }

        /// <summary>
        /// Update object user for admin 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/user/admin
        /// {
        ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "nameUser": "string",
        ///     "login": "string",
        ///     "password": "string",
        ///     "email": "string",
        ///     "phoneNumber": "string",
        ///     "role": "object"
        /// }
        /// </remarks>
        /// <param name="updateUserCommand">UpdateUserForAdminDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateUserForAdminDto updateUserCommand)
        {
            var command = mapper.Map<UpdateUserForAdminCommand>(updateUserCommand);
            command.CurrentUserId = UserId;
            var response = await Mediator.Send(command);
            if(!response)
                return Conflict();
            return NoContent();
        }
    }
}
