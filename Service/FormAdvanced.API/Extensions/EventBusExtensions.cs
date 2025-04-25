using FormAdvanced.BuildingBlocks.EventBus.Bus;
using FormAdvanced.BuildingBlocks.EventBus.MassTransit;
using FormAdvanced.BuildingBlocks.EventBus.MassTransit.Extensions;
using FormAdvanced.API.Constants;

namespace FormAdvanced.API.Extensions
{
    public static class EventBusExtensions
    {
        public static void AddEventBus(this WebApplicationBuilder builder)
        {
            //builder.Services.AddMassTransitServiceBus(builder.Configuration, ModuleConstants.ModuleAssemblies);
            builder.Services.AddMassTransitInMemory(ModuleConstants.ModuleAssemblies);

            builder.Services.AddTransient<IEventBus, MassTransitEventBus>();
        }
    }
}
