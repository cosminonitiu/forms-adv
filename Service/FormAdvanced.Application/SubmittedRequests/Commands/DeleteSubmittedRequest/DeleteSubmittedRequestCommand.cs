using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.DeleteSubmittedRequest
{
    public sealed record DeleteSubmittedRequestCommand(string Id, string Owner) : ICommand<Unit>;
}
