using FluentValidation;
using FormAdvanced.Application.FormRequest.Commands.AddFormRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.SaveDraftRequest
{
    public sealed class SaveDraftRequestCommandValidator : AbstractValidator<SaveDraftRequestCommand>
    {
        public SaveDraftRequestCommandValidator()
        {
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.OwnerName).NotEmpty();
            RuleFor(x => x.FormId).NotEmpty();
            RuleFor(x => x.FormOwnerId).NotEmpty();
        }
    }
}
