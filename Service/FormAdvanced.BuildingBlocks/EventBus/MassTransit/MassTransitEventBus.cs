using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormAdvanced.BuildingBlocks.EventBus.Bus;

namespace FormAdvanced.BuildingBlocks.EventBus.MassTransit
{
    public class MassTransitEventBus : IEventBus
    {
        private readonly IBus _bus;

        public MassTransitEventBus(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishAsync(object @event)
        {
            await _bus.Publish(@event);
        }

        public async Task PublishBatchAsync(IEnumerable<object> events)
        {
            await _bus.PublishBatch(events);
        }
    }
}
