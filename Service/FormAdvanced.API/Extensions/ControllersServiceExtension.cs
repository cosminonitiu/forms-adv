namespace FormAdvanced.API.Extensions
{
	public static class ControllersServiceExtension
	{
		public static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
		{
            // Adding Controllers from another assembly (You need an Assembly Reference
            /*
			var usersControllerAssembly = typeof(FormAdvanced.Modules.UserAccess.API.AssemblyReference).Assembly;
			builder.Services.AddControllers()
				.AddApplicationPart(usersControllerAssembly)
			*/

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
			return builder;
		}
	}
}
