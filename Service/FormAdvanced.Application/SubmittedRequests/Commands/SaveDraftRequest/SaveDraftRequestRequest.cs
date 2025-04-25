using FormAdvanced.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.SaveDraftRequest
{
    public sealed record SaveDraftRequestRequest(string FormId, string FormOwnerId, SubmittedSection[] Sections);
}
