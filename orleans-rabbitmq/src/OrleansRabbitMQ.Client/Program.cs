using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using Orleans;
using Serilog;

namespace OrleansRabbitMQ.Client
{
    class Program
    {
        public static Func<Task<IClusterClient>> ClusterClient;
        public static IServiceProvider Container { get; private set; }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        static async Task<int> Main(string[] args)
        {
            ClusterClient =()=> OrleansFactory.InitializeClient(args);

            var host = CreateWebHostBuilder(args).Build();

            Log.Logger?.Information("Host build complete");

            var returnCode = await ExecuteCommand(host, args);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Console.Read();
            }

            return returnCode;
        }
        
        static async Task<int> ExecuteCommand(IWebHost host, string[] args)
        {
            var executor = CommandExecutor.For(config =>
            {
                config.RegisterCommand<SendCommand>();
                config.RegisterCommand<SendLog>();
            });

            var container = host.Services;
            using (var scope = container.CreateScope())
            {
                Container = scope.ServiceProvider;
                return await executor.ExecuteAsync(args);
            }
        }
        
    }
}