using FormAdvanced.Application.SubmittedRequests.Commands.ApproveRequest;
using FormAdvanced.Application.SubmittedRequests.Commands.DeleteSubmittedRequest;
using FormAdvanced.Application.SubmittedRequests.Commands.RejectRequest;
using FormAdvanced.Application.SubmittedRequests.Commands.SaveDraftRequest;
using FormAdvanced.Application.SubmittedRequests.Commands.SubmitRequest;
using FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest;
using FormAdvanced.Application.SubmittedRequests.Queries.GetOwnedSubmittedRequests;
using FormAdvanced.Application.SubmittedRequests.Queries.GetSubmittedRequest;
using FormAdvanced.Application.SubmittedRequests.Queries.GetSubmittedRequestsToApprove;
using FormAdvanced.BuildingBlocks.Infrastructure.UserContext;
using FormAdvanced.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormAdvanced.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubmittedRequestsController : ControllerBase
    {
        private readonly ILogger<SubmittedRequestsController> _logger;
        private readonly IMediator _sender;
        private readonly IUserContext _userContext;

        public SubmittedRequestsController(ILogger<SubmittedRequestsController> logger, IMediator sender, IUserContext userContext)
        {
            _logger = logger;
            _sender = sender;
            _userContext = userContext;
        }

        [HttpGet("One")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmittedRequest))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<SubmittedRequest> GetSubmittedRequest([FromQuery] GetSubmittedRequestQuery query, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);
            return result;
        }

        [HttpGet("ToApprove")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SubmittedRequest>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<List<SubmittedRequest>> GetSubmittedRequestsToApprove([FromQuery] string Approver, CancellationToken cancellationToken)
        {
            var query = new GetSubmittedRequestsToApproveQuery(Approver);
            var result = await _sender.Send(query, cancellationToken);
            return result;
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SubmittedRequest>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<List<SubmittedRequest>> GetOwnedSubmittedRequests([FromQuery] string Owner,CancellationToken cancellationToken)
        {
            var query = new GetOwnedSubmittedRequestsQuery(Owner);
            var result = await _sender.Send(query, cancellationToken);
            return result;
        }


        [HttpPost()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        public async Task<string> SaveDraftRequest([FromBody] SaveDraftRequestRequest request, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var ownerName = _userContext.Name;
            var command = new SaveDraftRequestCommand(
                    Owner: owner,
                    OwnerName: ownerName,
                    FormId: request.FormId,
                    FormOwnerId: request.FormOwnerId,
                    Sections: request.Sections
            );
            var result = await _sender.Send(command, cancellationToken);
            return result;
        }

        
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubmittedRequest(string id, [FromBody] UpdateSubmittedRequestRequest request, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var command = new UpdateSubmittedRequestCommand(
                    Id: id,
                    Owner: owner,
                    Sections: request.Sections
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("Submit/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmitRequest(string id, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var command = new SubmitRequestCommand(
                    Id: id,
                    Owner: owner
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("Approve/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveRequest(string id, [FromBody] ApproveRequestRequest request, CancellationToken cancellationToken)
        {
            var approver = _userContext.UserId;
            var command = new ApproveRequestCommand(
                    Id: id,
                    Owner: request.Owner,
                    Approver: approver
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("Reject/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectRequest(string id, [FromBody] RejectRequestRequest request, CancellationToken cancellationToken)
        {
            var approver = _userContext.UserId;
            var command = new RejectRequestCommand(
                    Id: id,
                    Owner: request.Owner,
                    Approver: approver
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> DeleteSubmittedRequest(string id, [FromBody] DeleteSubmittedRequestRequest request, CancellationToken cancellationToken)
        {
            var command = new DeleteSubmittedRequestCommand(
                    Id: id,
                    Owner: request.Owner
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
