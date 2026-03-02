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
        /// <summary>
        /// Get all materials in module by module id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/material/all/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="moduleId">Module id guid</param>
        /// <returns>Returns MaterialListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{moduleId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<MaterialListVm>> GetAll(Guid moduleId)
        {
            var query = new GetAllMaterialQuery
            {
                ModuleId = moduleId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        /// <summary>
        /// Get info material by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/material/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Material id guid</param>
        /// <returns>Returns MaterialLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<MaterialLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsMaterialQuery
            {
                Id = id,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Delete object material by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/material/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Material id (guid)</param>
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
            var command = new DeleteMaterialCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Create object material 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/material
        ///{
        ///  "moduleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "title": "string",
        ///  "description": "string"
        ///}
        /// </remarks>
        /// <param name="createMaterialDto">CreateMaterialDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateMaterialDto createMaterialDto) 
        {
            var command = mapper.Map<CreateMaterialCommand>(createMaterialDto);
            command.CurrentUserId = UserId;
            var materialID = await Mediator.Send(command);
            return Ok(materialID);
        }

        /// <summary>
        /// Update object material 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/material
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "title": "string",
        ///  "description": "string"
        ///}        
        /// </remarks>
        /// <param name="updateMaterialDto">UpdateMaterialDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateMaterialDto updateMaterialDto)
        {
            var command = mapper.Map<UpdateMaterialCommand>(updateMaterialDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
