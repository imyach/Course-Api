using Application.Common.Commands.Users.CreateUser;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Commands.Users.UpdateUserForAdmin;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Users.GetUser;
using Application.Common.Queries.Users.GetUsersList;
using AutoMapper;
using CourseWebApi.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<UsersListVm>> GetAll()
        {
            var query = new GetAllUsersQuery()
            {
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<UserLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsUserQuery()
            {
                Id = id,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserDto createUserCommand)

        {
            var command = mapper.Map<CreateUserCommand>(createUserCommand);
            command.CurrentUserId = UserId;
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand()
            {

                Id = id,
                CurrentUserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserCommand)
        {
            var command = mapper.Map<UpdateUserCommand>(updateUserCommand);
            command.CurrentUserId = UserId;

            var response = await Mediator.Send(command);

            return Ok(new {token = response });
        }

        [HttpPut("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateUserDto updateUserCommand)
        {
            var command = mapper.Map<UpdateUserForAdminCommand>(updateUserCommand);
            command.CurrentUserId = UserId;
            var response = await Mediator.Send(command);
            if(!response)
                return Conflict();
            return NoContent();
        }
    }
}
