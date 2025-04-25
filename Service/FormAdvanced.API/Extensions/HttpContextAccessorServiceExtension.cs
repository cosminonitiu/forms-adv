using FormAdvanced.BuildingBlocks.Infrastructure.UserContext;

namespace FormAdvanced.API.Extensions
{
	public static class HttpContextAccessorServiceExtension
	{
		public static WebApplicationBuilder AddHttpContextAccessor(this WebApplicationBuilder builder)
		{
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IUserContext, UserContext>();
			return builder;
		}
	}
}
