using Application.Common.Commands.Reports.UserReport;
using Application.Common.Dtos.Reports.Users;
using AutoMapper;
using CourseWebApi.Models.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController(IMapper mapper) : BaseController
    {
        [HttpPost("users")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserReportReusltDto>> GenerateUserReport([FromBody]UserReportRequestDto userReportDto)
        {
            var command = mapper.Map<UserReportCommand>(userReportDto);
            var reportResult = await Mediator.Send(command);
            return Ok(reportResult);
        }
    }
}
