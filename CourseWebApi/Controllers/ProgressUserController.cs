using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Queries.ProgressUsers.GetProgressUser;
using Application.Common.Queries.ProgressUsers.GetProgressUserList;
using AutoMapper;
using CourseWebApi.Models.ProgressUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressUserController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Get progress users by user id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/progressUser/all
        /// </remarks>
        /// <param name="pageNumber">Page Number </param>
        /// <param name="userId">Viewed user id</param>
        /// <param name="pageSize">The number of items returned by the server</param>
        /// <param name="searchText">Text to search for items</param>
        /// <returns>Returns ProgressUserListVm </returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProgressUserListVm>> GetAll(Guid userId,
            [FromQuery] string searchText = null, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProgressUserQuery
            {
                UserId = userId,
                SearchText = searchText,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info progress user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/progressUser/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Progress user id guid</param>
        /// <returns>Returns ProgressUserLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProgressUserLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsProgressUserQuery
            {
                Id = id,
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Delete object progress user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/progressUser/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Progress user id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProgressUserCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Create object progressUser 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/progressUser
        ///  {
        ///    "courseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///  }
        /// </remarks>
        /// <param name="createProgressUserDto">CreateProgressUserDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProgressUserDto createProgressUserDto)
        {
            var command = mapper.Map<CreateProgressUserCommand>(createProgressUserDto);
            command.CurrentUserId = UserId;
            var progressUserId = await Mediator.Send(command);
            return Ok(progressUserId);
        }

        /// <summary>
        /// Update object progress user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/progressUser
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "status": "string",
        ///  "finishedAt": "2026-03-02T14:49:47.986Z"
        ///}
        /// </remarks>
        /// <param name="updateProgressUserDto">UpdateProgressUserDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateProgressUserDto updateProgressUserDto)
        {
            var command = mapper.Map<UpdateProgressUserCommand>(updateProgressUserDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
