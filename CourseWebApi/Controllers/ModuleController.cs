using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Commands.Modules.DeleteModule;
using Application.Common.Commands.Modules.UpdateModule;
using Application.Common.Dtos.Modules;
using Application.Common.Queries.Modules.GetModule;
using Application.Common.Queries.Modules.GetModuleList;
using AutoMapper;
using CourseWebApi.Models.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ModuleController(IMapper mapper) : BaseController
    {
        [HttpGet("All/{courseId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<ModuleListVm>> GetAll(Guid courseId)
        {
            var query = new GetAllModuleQuery
            {
                CourseId = courseId,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<ModuleLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsModuleQuery
            {
                Id =id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteModuleCommand
            {
                Id = id,
                CurrentUserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateModuleDto createModuleDto)
        {
            var command = mapper.Map<CreateModuleCommand>(createModuleDto);
            command.CurrentUserId = UserId;
            var moduleId = await Mediator.Send(command);
            return Ok(moduleId);
        }
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateModuleDto updateModuleDto)
        {
            var command = mapper.Map<UpdateModuleCommand>(updateModuleDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

    }
}
