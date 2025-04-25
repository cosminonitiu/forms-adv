using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.EventBus.Bus
{
    public interface IEventBus
    {
        Task PublishAsync(object @event);

        Task PublishBatchAsync(IEnumerable<object> events);
    }
}
