using System;
using System.Threading.Tasks;
using Orleans.Streams;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Client
{
    public class Observer : IAsyncObserver<Command>
    {
        public Task OnNextAsync(Command item, StreamSequenceToken token = null)
        {
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}