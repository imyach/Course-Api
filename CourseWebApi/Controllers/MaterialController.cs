using Application.Common.Commands.Materials.CreateMaterial;
using Application.Common.Commands.Materials.DeleteMaterial;
using Application.Common.Commands.Materials.UpdateMaterial;
using Application.Common.Dtos.Materials;
using Application.Common.Queries.Materials.GetMaterial;
using Application.Common.Queries.Materials.GetMaterialList;
using AutoMapper;
using CourseWebApi.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class MaterialController(IMapper mapper) : BaseController
    {
        [HttpGet("All/{moduleId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<MaterialListVm>> GetAll(Guid moduleId)
        {
            var query = new GetAllMaterialQuery
            {
                ModuleId = moduleId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<MaterialLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsMaterialQuery
            {
                Id = id,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteMaterialCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateMaterialDto createMaterialDto) 
        {
            var command = mapper.Map<CreateMaterialCommand>(createMaterialDto);
            command.CurrentUserId = UserId;
            var materialID = await Mediator.Send(command);
            return Ok(materialID);
        }

        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateMaterialDto updateMaterialDto)
        {
            var command = mapper.Map<UpdateMaterialCommand>(updateMaterialDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
