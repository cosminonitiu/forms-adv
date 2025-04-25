using FormAdvanced.Application.AD.Queries.GetAllUsers;
using FormAdvanced.Application.Common.Caching;
using FormAdvanced.Application.FormRequest.Commands.AddFormRequest;
using FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest;
using FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest;
using FormAdvanced.Application.FormRequests.Queries.GetFormRequest;
using FormAdvanced.Application.FormRequests.Queries.GetFormRequests;
using FormAdvanced.Application.FormRequests.Queries.GetOwnedFormRequests;
using FormAdvanced.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;

namespace FormAdvanced.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ADController : ControllerBase
    {
        private readonly ILogger<ADController> _logger;
        private readonly IMediator _sender;

        public ADController(ILogger<ADController> logger, IMediator sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet("AllUsers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(List<ADUser>))] // Type = typeof(UserCollectionResponse)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }     
    }
}
