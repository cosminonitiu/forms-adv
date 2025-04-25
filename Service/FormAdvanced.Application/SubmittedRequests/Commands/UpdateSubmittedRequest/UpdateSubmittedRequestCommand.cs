using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest
{
    public sealed record UpdateSubmittedRequestCommand(string Id, string Owner, SubmittedSection[] Sections) : ICommand<Unit>;
}
