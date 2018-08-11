using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Core
{
    [ImplicitStreamSubscription(Constants.StreamNameSpaceCustomers)]
    public class ReceiverGrain : Grain, IReceiverGrain
    {
        ILogger _logger;
        StreamSubscriptionHandle<Command> _subscriptionDefault;
        
        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _logger = ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger($"{typeof(ReceiverGrain).FullName}.{this.GetPrimaryKey()}");
            _logger.LogInformation($"OnActivateAsync [{RuntimeIdentity}],[{IdentityString}][{this.GetPrimaryKey()}] from thread {Thread.CurrentThread.Name}");

            _subscriptionDefault = await GetStreamProvider(Constants.StreamProviderNameDefault)
                .GetStream<Command>(this.GetPrimaryKey(), Constants.StreamNameSpaceCustomers)
                .SubscribeAsync(OnNextAsync);
        }

        public override async Task OnDeactivateAsync()
        {
            _logger.LogInformation($"OnDeactivateAsync [{RuntimeIdentity}],[{IdentityString}][{this.GetPrimaryKey()}] from thread {Thread.CurrentThread.Name}");
            
            await _subscriptionDefault.UnsubscribeAsync();
            await base.OnDeactivateAsync();
        }
        
        async Task OnNextAsync(Command exampleMessage, StreamSequenceToken token = null)
        {
            _logger.LogInformation($"OnNextAsync in #{exampleMessage.Value} [{RuntimeIdentity}],[{IdentityString}][{this.GetPrimaryKey()}] from thread {Thread.CurrentThread.Name}");
            
            DeactivateOnIdle();
        }      
    }
}