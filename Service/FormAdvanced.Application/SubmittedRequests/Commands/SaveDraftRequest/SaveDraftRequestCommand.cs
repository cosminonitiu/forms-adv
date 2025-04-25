using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.SaveDraftRequest
{
    public sealed record SaveDraftRequestCommand(
        string Owner, string OwnerName, string FormId, string FormOwnerId, SubmittedSection[] Sections
        ) : ICommand<string>;
}
