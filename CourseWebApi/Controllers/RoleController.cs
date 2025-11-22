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
    public class RoleController(IMapper mapper) : BaseController
    {
        [HttpGet("roleAll")]
        public async Task<ActionResult<RoleListVm>> GetAll()
        {
            var query = new GetAllRolesQuery()
            {
                UserId = UserId,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
    

        [HttpGet("roleById/{id}")]
        public async Task<ActionResult<RoleDetailsVm>> GetById(Guid id)
        {
            var query = new GetDetailsRoleQuery()
            {
                UserId = UserId,
                Id = id
                
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }


    }
}
