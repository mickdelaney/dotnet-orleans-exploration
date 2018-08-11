using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Core
{
    public class LoggerGrain : Grain, ILoggerGrain
    {
        readonly ILoggerFactory _loggerFactory;
        
        ILogger _logger;

        public LoggerGrain(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            
            _logger = _loggerFactory.CreateLogger($"{typeof(CustomerCommandHandlerGrain).FullName}.{this.GetPrimaryKey()}");
        }
        
        public Task Log(string message)
        {
            _logger.LogInformation($"{this.GetPrimaryKey()} Received: {message}");
            
            return Task.CompletedTask;
        }
    }
}