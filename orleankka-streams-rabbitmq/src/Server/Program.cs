using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Streams;
using Orleankka.Cluster;
using Shared;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Constants.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .UseLocalhostClustering()
                .AddMemoryGrainStorage("PubSubStore")
                .AddRabbitMqStream(Constants.StreamProviderNameDefault, configurator =>
                {
                    configurator.ConfigureRabbitMq
                    (
                        host: "elevate.rabbit.local",
                        port: Constants.RmqProxyPort,
                        virtualHost: "/",
                        user: "elevate",
                        password: "elevate",
                        queueName: Constants.StreamNameSpaceCustomers
                    );
                    configurator.ConfigureCache(cacheSize: 100, cacheFillingTimeout: TimeSpan.FromSeconds(10));
                    configurator.ConfigureStreamPubSub(StreamPubSubType.ExplicitGrainBasedAndImplicit);
                    configurator.ConfigurePullingAgent
                    (
                        ob => ob.Configure
                        (
                            options => { options.GetQueueMsgsTimerPeriod = TimeSpan.FromMilliseconds(100); }
                        )
                    );
                })
                .ConfigureServices(services =>
                {
//                    services.AddSingleton(container => ExampleConfiguration.WorkflowConfig);
//                    services.AddTransient<Func<NpgsqlConnection>>(container => DatabaseManager.GetConnection);
//        
//                    WorkflowInstaller.Install(services);
//                    ExampleInstaller.Install(services);
                })
                .Configure<EndpointOptions>(options =>
                    options.AdvertisedIPAddress = IPAddress.Loopback
                )
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(Join).Assembly)
                        .WithReferences()
                )
                .ConfigureLogging(logging => logging.AddConsole())
                .UseOrleankka();
            
            var silo = builder.Build();
            
            await silo.StartAsync();

            Console.WriteLine("Press Enter to close.");
            Console.ReadLine();

            // shut the silo down after we are done.
            await silo.StopAsync();
        }
    }
}