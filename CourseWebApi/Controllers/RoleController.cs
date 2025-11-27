using Application.Common.Dtos.Roles;
using Application.Common.Queries.Roles.GetRole;
using Application.Common.Queries.Roles.GetRoleList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<RoleListVm>> GetAll()
        {
            var query = new GetAllRolesQuery()
            {
                CurrentUserId = UserId,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
    

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleLookupDto>> GetById(Guid id)
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
