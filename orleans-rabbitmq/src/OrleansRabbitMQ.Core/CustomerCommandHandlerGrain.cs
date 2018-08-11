using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Core
{
    [ImplicitStreamSubscription(Constants.StreamNameSpaceCustomers)]
    public class CustomerCommandHandlerGrain : Grain, ICommandHandlerGrain
    {
        readonly ILoggerFactory _loggerFactory;
        
        ILogger _logger;
        StreamSubscriptionHandle<Command> _subscription;

        public CustomerCommandHandlerGrain(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            
            _logger = _loggerFactory.CreateLogger($"{typeof(CustomerCommandHandlerGrain).FullName}.{this.GetPrimaryKey()}");
            _logger.LogInformation($"CustomerCommandHandler OnActivateAsync [{RuntimeIdentity}],[{IdentityString}][{this.GetPrimaryKey()}] from thread {Thread.CurrentThread.Name}");

            _subscription = await GetStreamProvider(Constants.StreamProviderNameDefault)
                .GetStream<Command>(this.GetPrimaryKey(), Constants.StreamNameSpaceCustomers)
                .SubscribeAsync(OnNextAsync);
        }

        public override async Task OnDeactivateAsync()
        {
            _logger.LogInformation($"CustomerCommandHandler OnDeactivateAsync [{RuntimeIdentity}],[{IdentityString}][{this.GetPrimaryKey()}] from thread {Thread.CurrentThread.Name}");
            
            await _subscription.UnsubscribeAsync();
            await base.OnDeactivateAsync();
        }
        
        async Task OnNextAsync(Command exampleMessage, StreamSequenceToken token = null)
        {
            _logger.LogInformation($"CustomerCommandHandler OnNextAsync in #{exampleMessage.Value} [{RuntimeIdentity}],[{IdentityString}][{this.GetPrimaryKey()}] from thread {Thread.CurrentThread.Name}");
            
            DeactivateOnIdle();
        }      
    }
}