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
        /// <summary>
        /// Get all progress materials by user id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/progressMaterial/all
        /// </remarks>
        /// <param name="progressModuleId">Progress module id guid</param>
        /// <returns>Returns ProgressMaterialListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{progressModuleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProgressMaterialListVm>> GetAll(Guid progressModuleId)
        {
            var query = new GetAllProgressMaterialQuery
            {
                CurrentUserId = UserId,
                ProgressModuleId = progressModuleId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info progress material by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/progressMaterial/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Progress material id guid</param>
        /// <returns>Returns ProgressMaterialLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Delete object progress material by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/progressMaterial/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Progress material id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Update object progress material
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/progressMaterial
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "status": "string",
        ///  "startedAt": "2026-03-02T14:23:33.965Z"
        ///}
        /// </remarks>
        /// <param name="updateProgressMaterialDto">UpdateProgressMaterialDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateProgressMaterialDto updateProgressMaterialDto)
        {
            var command = mapper.Map<UpdateProgressMaterialCommand>(updateProgressMaterialDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}

