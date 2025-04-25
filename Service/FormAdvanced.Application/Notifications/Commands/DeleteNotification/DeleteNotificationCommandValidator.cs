using FluentValidation;
using FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.Notifications.Commands.DeleteNotification
{
    public sealed class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
    {
        public DeleteNotificationCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
        }
    }
}
