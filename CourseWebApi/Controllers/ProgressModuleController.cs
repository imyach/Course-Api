using Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials;
using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
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
        /// <summary>
        /// Get all progress modules by user id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/progressModule/all
        /// </remarks>
        /// <param name="progressUserId">Progress user id guid</param>
        /// <returns>Returns ProgressModuleListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{progressUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProgressModuleListVm>> GetAll(Guid progressUserId)
        {
            var query = new GetAllProgressModuleQuery
            {
                CurrentUserId = UserId,
                ProgressUserId = progressUserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info progress module by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/progressModule/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Progress module id guid</param>
        /// <returns>Returns ProgressModuleLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Delete object progress module by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/progressModule/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Progress module id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Update object progress module
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/progressModule
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "status": "string",
        ///  "startedAt": "2026-03-02T14:32:49.260Z"
        ///}
        /// </remarks>
        /// <param name="updateProgressModuleDto">UpdateProgressModuleDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateProgressModuleDto updateProgressModuleDto)
        {
            var command = mapper.Map<UpdateProgressModuleCommand>(updateProgressModuleDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}