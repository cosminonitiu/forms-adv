using FluentValidation;
using MediatR;
using FormAdvanced.API.Middleware;
using FormAdvanced.BuildingBlocks.Application.Configuration.Validation;

namespace FormAdvanced.API.Extensions
{
	public static class MiddlewareServiceExtension
	{
		public static WebApplicationBuilder AddMiddleware(this WebApplicationBuilder builder)
		{
            var currentAssembly = typeof(Program).Assembly;
            var applicationAssembly = typeof(FormAdvanced.Application.AssemblyReference).Assembly;

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

			builder.Services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(currentAssembly);
				cfg.RegisterServicesFromAssembly(applicationAssembly);
			});

			builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

			builder.Services.AddValidatorsFromAssembly(currentAssembly);
            builder.Services.AddValidatorsFromAssembly(applicationAssembly);

            return builder;
		}
	}
}
