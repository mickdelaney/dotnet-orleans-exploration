using System;
using System.Threading.Tasks;
using Orleans;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Core
{
    public class LoggerGrain : Grain, ILoggerGrain
    {
        public async Task Log(string message)
        {
            await Console.Out.WriteLineAsync($"{this.GetPrimaryKey()} Received: {message}");
        }
    }
}