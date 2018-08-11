using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleankka.Client;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Shared;

namespace Client
{
    public class OrleansFactory
    {
        public static event Action OnClusterConnectionLost = () => {};
        
        public static async Task<IClusterClient> InitializeClient(string[] args)
        {
            int initializeCounter = 0;

            var initSucceed = false;
            
            while (!initSucceed)
            {
                try
                {
                    var builder = new ClientBuilder();
                    var client = builder
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = Constants.ClusterId;
                            options.ServiceId = Constants.ServiceId;
                        })
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
                        })
                        .UseLocalhostClustering()
                        .ConfigureServices(x => x
                            .AddSingleton<ConnectionToClusterLostHandler>((s, e) => OnClusterConnectionLost()))
                        .ConfigureApplicationParts(parts => 
                            parts.AddApplicationPart(typeof(IChatUser).Assembly).WithReferences()
                        )
                        .ConfigureLogging(logging => logging.AddConsole())
                        .UseOrleankka()
                        .Build();
                    
                    await client.Connect();
                    
                    initSucceed = client.IsInitialized;
                    
                    if (initSucceed)
                    {
                        return client;
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    initSucceed = false;
                }

                if (initializeCounter++ > 10)
                {
                    return null;
                }

                Console.WriteLine("Client Init Failed. Sleeping 5s...");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            return null;
        }
    }
}