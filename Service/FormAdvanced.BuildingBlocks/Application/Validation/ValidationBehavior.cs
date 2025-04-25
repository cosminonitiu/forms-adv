using FluentValidation;
using MediatR;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using ValidationException = FormAdvanced.BuildingBlocks.Domain.Exceptions.ValidationException;

namespace FormAdvanced.BuildingBlocks.Application.Configuration.Validation
{
    public sealed class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
         where TRequest : class, ICommand<TResult>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {

            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var errorsDictionary = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);

            if (errorsDictionary.Any())
            {
                throw new ValidationException(errorsDictionary);
            }

            return await next();
        }

    }
}
