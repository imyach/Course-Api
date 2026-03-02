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
        /// <summary>
        /// Get all modules in course by course id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/module/all/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="courseId">Course id guid</param>
        /// <returns>Returns ModuleListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{courseId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleListVm>> GetAll(Guid courseId)
        {
            var query = new GetAllModuleQuery
            {
                CourseId = courseId,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info module by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/module/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Module id guid</param>
        /// <returns>Returns ModuleLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsModuleQuery
            {
                Id =id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Delete object module by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/module/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Module id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Create object module 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/module
        ///{
        ///  "title": "string",
        ///  "description": "string",
        ///  "courseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///}
        /// </remarks>
        /// <param name="createModuleDto">CreateModuleDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateModuleDto createModuleDto)
        {
            var command = mapper.Map<CreateModuleCommand>(createModuleDto);
            command.CurrentUserId = UserId;
            var moduleId = await Mediator.Send(command);
            return Ok(moduleId);
        }

        /// <summary>
        /// Update object module 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/module
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "title": "string",
        ///  "description": "string"
        ///}        
        /// </remarks>
        /// <param name="updateModuleDto">UpdateModuleDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateModuleDto updateModuleDto)
        {
            var command = mapper.Map<UpdateModuleCommand>(updateModuleDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

    }
}
