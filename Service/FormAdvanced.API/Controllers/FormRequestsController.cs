using FormAdvanced.Application.Common.Caching;
using FormAdvanced.Application.FormRequest.Commands.AddFormRequest;
using FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest;
using FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest;
using FormAdvanced.Application.FormRequests.Queries.GetFormRequest;
using FormAdvanced.Application.FormRequests.Queries.GetFormRequests;
using FormAdvanced.Application.FormRequests.Queries.GetOwnedFormRequests;
using FormAdvanced.BuildingBlocks.Infrastructure.UserContext;
using FormAdvanced.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormAdvanced.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormRequestsController : ControllerBase
    {
        private readonly ILogger<FormRequestsController> _logger;
        private readonly IMediator _sender;
        private readonly IUserContext _userContext;

        public FormRequestsController(ILogger<FormRequestsController> logger, IMediator sender, IUserContext userContext)
        {
            _logger = logger;
            _sender = sender;
            _userContext = userContext;
        }

        [HttpGet("One")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FormRequest))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetFormRequest([FromQuery] GetFormRequestQuery query, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);

            // Add cache information to response headers
            Response.Headers.Add("X-Data-Source", result.IsFromCache ? "Cache" : "CosmosDB");
            if (result.IsFromCache && result.CachedAt.HasValue)
            {
                Response.Headers.Add("X-Cached-At", result.CachedAt.Value.ToString("o"));
            }

            return Ok(result.Data);
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FormRequest>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetFormRequests(CancellationToken cancellationToken)
        {
            var query = new GetFormRequestsQuery();
            var result = await _sender.Send(query, cancellationToken);
            
            // Add cache information to response headers
            Response.Headers.Add("X-Data-Source", result.IsFromCache ? "Cache" : "CosmosDB");
            if (result.IsFromCache && result.CachedAt.HasValue)
            {
                Response.Headers.Add("X-Cached-At", result.CachedAt.Value.ToString("o"));
            }
            
            return Ok(result.Data);
        }

        [HttpGet("Managed")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FormRequest>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOwnedFormRequests(CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var query = new GetOwnedFormRequestsQuery(owner);
            var result = await _sender.Send(query, cancellationToken);
            
            // Add cache information to response headers
            Response.Headers.Add("X-Data-Source", result.IsFromCache ? "Cache" : "CosmosDB");
            if (result.IsFromCache && result.CachedAt.HasValue)
            {
                Response.Headers.Add("X-Cached-At", result.CachedAt.Value.ToString("o"));
            }
            
            return Ok(result.Data);
        }

        [HttpPost()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        public async Task<string> AddFormRequest([FromBody] AddFormRequestsRequest request, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var command = new AddFormRequestsCommand(
                    Owner: owner,
                    Name: request.Name,
                    Description: request.Description,
                    Color: request.Color,
                    Icon: request.Icon,
                    HideSections: request.HideSections
            );
            var result = await _sender.Send(command, cancellationToken);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFormRequest(string id, [FromBody] UpdateFormRequestsRequest request, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var command = new UpdateFormRequestsCommand(
                    Id: id,
                    Owner: owner,
                    Sections: request.Sections
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> DeleteFormRequest(string id, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var command = new DeleteFormRequestsCommand(
                    Id: id,
                    Owner: owner
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
