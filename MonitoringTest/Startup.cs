using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(MonitoringTest.Startup))]

namespace MonitoringTest
{
    using Microsoft.Extensions.DependencyInjection;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            builder.Services.AddOpenTelemetryTracing((builder) => builder
                .AddAspNetCoreInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MonitoringTest"))
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:4317");
                })
            );
        }
    }
}
