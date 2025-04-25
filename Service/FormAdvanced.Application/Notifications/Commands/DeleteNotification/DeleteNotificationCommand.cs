using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.Notifications.Commands.DeleteNotification
{
    public sealed record DeleteNotificationCommand(string Id, string Owner) : ICommand<Unit>;
}
