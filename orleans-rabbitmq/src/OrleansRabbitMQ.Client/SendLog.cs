using System;
using System.Threading.Tasks;
using Oakton;
using OrleansRabbitMQ.Interfaces;

namespace OrleansRabbitMQ.Client
{
    [Description("Send Log", Name = "send-log")]
    public class SendLog : OaktonAsyncCommand<LogMessage>
    {  
        public SendLog()
        {
            Usage("Message").Arguments(x => x.Message);
        }
        
        public override async Task<bool> Execute(LogMessage input)
        {
            var cluster = await Program.ClusterClient();
            var id = Guid.NewGuid();
            var logger = cluster.GetGrain<ILoggerGrain>(id);
            
            await logger.Log(input.Message);

            return true;
        }
    }
}