using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace FormAdvanced.API.Extensions
{
	public static class SwaggerServiceExtension
	{
		public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
		{
			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. \n\n Enter \"Bearer <token>\" in the text input below. \n Example: 'Bearer 12345abcdef'",
					Name = "Authorization",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
				c.TagActionsBy(api =>
				{
					if (api.GroupName != null)
					{
						return new[] { api.GroupName };
					}

					var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
					if (controllerActionDescriptor != null)
					{
						return new[] { controllerActionDescriptor.ControllerName };
					}

					throw new InvalidOperationException("Unable to determine tag for endpoint.");
				});
				c.DocInclusionPredicate((name, api) => true);
			});
			return builder;
		}
	}
}
