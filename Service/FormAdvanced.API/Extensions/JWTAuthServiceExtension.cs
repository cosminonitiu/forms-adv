using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FormAdvanced.BuildingBlocks.Infrastructure.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace FormAdvanced.API.Extensions
{
	public static class JWTAuthServiceExtension
	{
		public static WebApplicationBuilder AddJWTAuth(this WebApplicationBuilder builder)
		{
            // Adds Microsoft Identity platform (Azure AD B2C) support to protect this Api
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(
                // 1st lambda: bind MicrosoftIdentityOptions
                options =>
                {
                    builder.Configuration.Bind("AzureAd", options);
                    options.TokenValidationParameters.NameClaimType = "name";
                },
                // 2nd lambda: bind JwtBearerOptions
                jwtOptions =>
                {
                    builder.Configuration.Bind("AzureAd", jwtOptions);
                }
              );

            return builder;
		}
	}
}
