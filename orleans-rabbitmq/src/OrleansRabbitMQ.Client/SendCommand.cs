using System;
using System.Threading.Tasks;
using Oakton;
using Orleans.Streams;
using OrleansRabbitMQ.Core;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Client
{
    [Description("Send Command", Name = "send-command")]
    public class SendCommand : OaktonAsyncCommand<NoInput>
    {
        public override async Task<bool> Execute(NoInput input)
        {
            var cluster = await Program.ClusterClient();
            var client = cluster.GetStreamProvider(Constants.StreamProviderNameDefault);
            var stream = client.GetStream<Command>(Guid.NewGuid(), Constants.StreamNameSpaceCustomers);

            StreamSubscriptionHandle<Command> subscriptionHandle = await stream.SubscribeAsync(new Observer());

            await Console.Out.WriteLineAsync("Sending Command");
            
            await stream.OnNextAsync(new Command
            {
                Value = $"TEST-{DateTime.Now.ToShortTimeString()}"
            });

            return true;
        }
    }
}