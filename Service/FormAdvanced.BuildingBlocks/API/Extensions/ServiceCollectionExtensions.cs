using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using FormAdvanced.BuildingBlocks.API.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FormAdvanced.BuildingBlocks.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {

                    Description = "JWT Authorization header using the Bearer scheme. \n\n\tEnter \"Bearer <token>\" in the text input below. \n\tExample: 'Bearer 12345abcdef'",

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
                        new string[] {}
                    }
                });
            });
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }

        public static void AddAppInsights(this IServiceCollection services)
        {
            if (string.IsNullOrEmpty(
                    Environment.GetEnvironmentVariable(EnvironmentVariableConstants.AppInsightsConnectionString)))
            {
                return;
            }

            services.AddApplicationInsightsTelemetry();

            services.AddApplicationInsightsKubernetesEnricher();
        }

        public static TClass AddConfiguration<TClass>(this WebApplicationBuilder builder, string name) where TClass : class
        {
            builder.Services.Configure<TClass>(builder.Configuration.GetSection(name));

            return builder.Configuration.GetSection(name).Get<TClass>();
        }

        public static void AddEnvironmentVariables(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables("WP_");
        }
        public static IHostBuilder AddEnvironmentVariables(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(c => c.AddEnvironmentVariables("WP_"));

            return builder;
        }
    }
}
