using Application.Common.Dtos.Roles;
using Application.Common.Queries.Roles.GetRole;
using Application.Common.Queries.Roles.GetRoleList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]

    [Authorize]
    public class RoleController : BaseController
    {
        /// <summary>
        /// Get all roles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/role/all
        /// </remarks>
        /// <returns>Returns RoleListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoleListVm>> GetAll()
        {
            var query = new GetAllRolesQuery()
            {
                CurrentUserId = UserId,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info role by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/role/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Role id guid</param>
        /// <returns>Returns RoleLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoleLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsRoleQuery()
            {
                CurrentUserId = UserId,
                Id = id
                
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }


    }
}
