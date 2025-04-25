using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace FormAdvanced.BuildingBlocks.API.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInsightsLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging((ctx, builder) =>
            {
                builder.AddConsole();

                builder.AddApplicationInsights();
            });

            return hostBuilder;
        }
    }
}
