using Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials;
using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterials;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList;
using Application.Common.Queries.ProgressUsers.GetProgressUser;
using Application.Common.Queries.ProgressUsers.GetProgressUserList;
using AutoMapper;
using CourseWebApi.Models.ProgressMaterial;
using CourseWebApi.Models.ProgressUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressMaterialController(IMapper mapper) : BaseController
    {
        [HttpGet("All/{progressModuleId}")]
        public async Task<ActionResult<ProgressMaterialListVm>> GetAll(Guid userId, Guid progressModuleId)
        {
            var query = new GetAllProgressMaterialQuery
            {
                CurrentUserId = UserId,
                ProgressModuleId = progressModuleId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressMaterialLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsProgressMaterialQuery
            {
                Id = id,
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProgressMaterialCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProgressMaterialDto updateProgressMaterialDto)
        {
            var command = mapper.Map<UpdateProgressMaterialCommand>(updateProgressMaterialDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}

