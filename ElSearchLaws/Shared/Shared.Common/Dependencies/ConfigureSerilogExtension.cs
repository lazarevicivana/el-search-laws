using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Http;

namespace Shared.Dependencies;

public static class ConfigureSerilogExtension
{
    public static void ConfigureSerilog(this IHostBuilder hostBuilder,
        string httpSinkRequestUri,
        string serviceName)
    {
        hostBuilder.UseSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration
                .WriteTo.DurableHttpUsingTimeRolledBuffers(
                    httpSinkRequestUri,
                    bufferRollingInterval: BufferRollingInterval.Month,
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    logEventsInBatchLimit: 1000)
                .Enrich.WithProperty("serviceName", serviceName);
            
            loggerConfiguration
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .Destructure.ToMaximumDepth(4)
                .Destructure.ToMaximumStringLength(100)
                .Destructure.ToMaximumStringLength(100)
                .Destructure.ToMaximumCollectionCount(10);
        });
    }
}