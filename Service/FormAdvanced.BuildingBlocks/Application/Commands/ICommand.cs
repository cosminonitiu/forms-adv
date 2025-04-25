using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Application.Configuration.Commands
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}
