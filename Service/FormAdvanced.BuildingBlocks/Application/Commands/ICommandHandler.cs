using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Application.Configuration.Commands
{
    public interface ICommandHandler<in TCommand, TResult> :
       IRequestHandler<TCommand, TResult>
       where TCommand : ICommand<TResult>
    {
    }
}
