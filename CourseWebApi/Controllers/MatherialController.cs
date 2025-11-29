using Application.Common.Commands.Matherials.CreateMatherial;
using Application.Common.Commands.Matherials.DeleteMatherial;
using Application.Common.Commands.Matherials.UpdateMatherial;
using Application.Common.Dtos.Matherials;
using Application.Common.Queries.Matherials.GetMatherial;
using Application.Common.Queries.Matherials.GetMatherialList;
using AutoMapper;
using CourseWebApi.Models.Matherial;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class MatherialController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<MatherialListVm>> GetAll()
        {
            var query = new GetAllMatherialQuery
            {
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatherialLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsMatherialQuery
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
            var command = new DeleteMatherialCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateMatherialDto createMatherialDto) 
        {
            var command = mapper.Map<CreateMatherialCommand>(createMatherialDto);
            command.CurrentUserId = UserId;
            var matherialID = await Mediator.Send(command);
            return Ok(matherialID);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMatherialDto updateMatherialDto)
        {
            var command = mapper.Map<UpdateMatherialCommand>(updateMatherialDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
