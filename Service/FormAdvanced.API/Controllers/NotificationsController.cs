using FormAdvanced.Application.Notifications.Commands.DeleteNotification;
using FormAdvanced.Application.Notifications.Queries.GetOwnedNotifications;
using FormAdvanced.BuildingBlocks.Infrastructure.UserContext;
using FormAdvanced.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormAdvanced.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;
        private readonly IMediator _sender;
        private readonly IUserContext _userContext;

        public NotificationsController(ILogger<NotificationsController> logger, IMediator sender, IUserContext userContext)
        {
            _logger = logger;
            _sender = sender;
            _userContext = userContext;
        }


        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FormRequest>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOwnedNotifications(CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var query = new GetOwnedNotificationsQuery(owner);
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

       
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // CASES: Validation errors
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> DeleteNotification(string id, CancellationToken cancellationToken)
        {
            var owner = _userContext.UserId;
            var command = new DeleteNotificationCommand(
                    Id: id,
                    Owner: owner
            );
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
