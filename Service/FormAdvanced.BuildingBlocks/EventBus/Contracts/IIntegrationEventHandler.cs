using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace FormAdvanced.BuildingBlocks.EventBus.Contracts
{
    public interface IIntegrationEventHandler<TMessage> : IConsumer<TMessage> where TMessage : IntegrationEvent
    {
    }
}
