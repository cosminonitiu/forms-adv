using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisePerform.BuildingBlocks.Domain.Contracts.Example.Events
{
	public record ExampleEvent(Guid Id);
    // In the emitter you have to inject private readonly IEventBus _eventBus;
    // await _eventBus.PublishAsync(new ExampleEvent(item.Id));
}
