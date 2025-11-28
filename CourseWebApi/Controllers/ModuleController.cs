using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Commands.Modules.DeleteModule;
using Application.Common.Commands.Modules.UpdateModule;
using Application.Common.Dtos.Modules;
using Application.Common.Queries.Modules.GetModule;
using Application.Common.Queries.Modules.GetModuleList;
using AutoMapper;
using CourseWebApi.Models.Module;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ModuleController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<ModuleListVm>> GetAll()
        {
            var query = new GetAllModuleQuery
            {
                CurrentUserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleLookupDto>> GetAll(Guid id)
        {
            var query = new GetDetailsModuleQuery
            {
                Id =id,
                CurrentUserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
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
        public async Task<ActionResult<Guid>> Delete([FromBody] CreateModuleDto createModuleDto)
        {
            var command = mapper.Map<CreateModuleCommand>(createModuleDto);
            command.CurrentUserId = UserId;
            var moduleId = await Mediator.Send(command);
            return Ok(moduleId);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateModuleDto updateModuleDto)
        {
            var command = mapper.Map<UpdateModuleCommand>(updateModuleDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

    }
}
