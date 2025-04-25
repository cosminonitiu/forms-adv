using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FormAdvanced.BuildingBlocks.EventBus.Contracts;
using FormAdvanced.BuildingBlocks.EventBus.Options;


namespace FormAdvanced.BuildingBlocks.EventBus.MassTransit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMassTransitInMemory(this IServiceCollection services, Assembly[] moduleAssemblies)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(moduleAssemblies);

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            //services.AddMassTransitHostedService(true);
        }
        public static void AddMassTransitServiceBus(this IServiceCollection services, IConfiguration config, Assembly[] moduleAssemblies)
        {
            var options = config.GetSection(ServiceBusOptions.SB).Get<ServiceBusOptions>();

            //var handlers = moduleAssemblies.Select(a =>
            //        typeof(IIntegrationEventHandler<>).GetGenericArgumentAndImplementationOfType(a))
            //    .Aggregate((d1, d2) => d1.Concat(d2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

            services.AddMassTransit(x =>
            {
                x.AddConsumers(moduleAssemblies);

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(options.Uri, busConfigure =>
                    {
                        busConfigure.TokenCredential = new DefaultAzureCredential();
                    });

                    cfg.ConfigureEndpoints(context);

                });
            });


            //services.AddMassTransitHostedService(true);
        }
    }
}
