using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Common.Queries.Courses.GetCreatedCourse;
using AutoMapper;
using CourseWebApi.Models.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CourseController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Get published courses elements
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/course/all
        /// </remarks>
        /// <param name="pageNumber">Page Number </param>
        /// <param name="pageSize">The number of items returned by the server</param>
        /// <param name="searchText">Text to search for items</param>
        /// <returns>Returns CourseListVm</returns>
        /// <response code="200">Siccess</response>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CourseListVm>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchText = null)
        {
            var query = new GetAllCourseQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchText = searchText
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get drafted courses elements
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/course/Drafted
        /// </remarks>
        /// <param name="pageNumber">Page Number </param>
        /// <param name="pageSize">The number of items returned by the server</param>
        /// <returns>Returns CourseListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpGet("Drafted")]
        [Authorize(Roles = "Admin,Couch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CourseListVm>> GetAllCreated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCreatedCoursesQuery
            {
                CurrentUserId = UserId,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info course by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/course/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Course id guid</param>
        /// <returns>Returns CourseLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Couch,Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CourseLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsCourseQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Create object course 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/course
        /// {
        ///     "title": "string",
        ///     "description": "string"
        /// }
        /// </remarks>
        /// <param name="createCourseDto">CreateCourseDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Couch")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCourseDto createCourseDto)
        {
            var command = mapper.Map<CreateCourseCommand>(createCourseDto);
            command.CurrentUserId = UserId;
            var commandId = await Mediator.Send(command);
            return Ok(commandId);
        }

        /// <summary>
        /// Update object course 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/course
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "title": "string",
        ///  "description": "string",
        ///  "status": "string"
        ///}
        /// </remarks>
        /// <param name="updateCourseDto">UpdateCourseDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Admin,Couch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateCourseDto updateCourseDto )
        {
            var command = mapper.Map<UpdateCourseCommand>(updateCourseDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }


        /// <summary>
        /// Delete object course by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/course/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Course id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin,Couch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var command = new DeleteCourseCommand
            {
                Id = id,
                CurrentUserId = UserId
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
