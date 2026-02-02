using Application.Common.Commands.ProgressMaterials.CreateProgressMaterials;
using Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials;
using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Commands.ProgressModules.CreateProgressModules;
using Application.Common.Commands.ProgressModules.DeleteProgressModules;
using Application.Common.Commands.ProgressModules.UpdateProgressModules;
using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterials;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList;
using Application.Common.Queries.ProgressModules.GetProgressModules;
using Application.Common.Queries.ProgressModules.GetProgressModulesList;
using AutoMapper;
using CourseWebApi.Models.ProgressMaterial;
using CourseWebApi.Models.ProgressModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressModuleController(IMapper mapper) : BaseController
    {
        [HttpGet("All/{userId}")]
        public async Task<ActionResult<ProgressModuleListVm>> GetAll(Guid userId, Guid progressUserId)
        {
            var query = new GetAllProgressModuleQuery
            {
                CurrentUserId = UserId,
                UserId = userId,
                ProgressUserId = progressUserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressModuleLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsProgressModuleQuery
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
            var command = new DeleteProgressModuleCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProgressModuleDto createProgressModuleDto)
        {
            var command = mapper.Map<CreateProgressModuleCommand>(createProgressModuleDto);
            command.CurrentUserId = UserId;
            var progressModuleId = await Mediator.Send(command);
            return Ok(progressModuleId);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProgressModuleDto updateProgressModuleDto)
        {
            var command = mapper.Map<UpdateProgressModuleCommand>(updateProgressModuleDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}