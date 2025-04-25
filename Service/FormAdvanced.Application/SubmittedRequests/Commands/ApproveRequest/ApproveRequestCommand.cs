using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.ApproveRequest
{
    public sealed record ApproveRequestCommand(string Id, string Owner, string Approver) : ICommand<Unit>;
}
