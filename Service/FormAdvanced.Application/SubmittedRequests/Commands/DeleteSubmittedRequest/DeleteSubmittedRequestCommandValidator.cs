using FluentValidation;
using FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.DeleteSubmittedRequest
{
    public sealed class DeleteSubmittedRequestCommandValidator : AbstractValidator<DeleteSubmittedRequestCommand>
    {
        public DeleteSubmittedRequestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
        }
    }
}
