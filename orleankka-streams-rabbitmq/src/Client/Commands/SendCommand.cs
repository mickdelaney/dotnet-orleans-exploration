using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Oakton;
using Orleankka.Client;

namespace Client.Commands
{
    [Description("Send Command", Name = "send-command")]
    public class SendCommand : OaktonAsyncCommand<NoInput>
    {
        public override async Task<bool> Execute(NoInput input)
        {
            const string room = "Orleankka";
            
            var cluster = await Program.ClusterClient();
            var system = cluster.ActorSystem();
            
            var client = new ChatClient(system, "mick", room);
            
            await client.Join();

            foreach (var i in Enumerable.Range(1, 20))
            {
                await client.Say($" Hello a {i}th time!! ");

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            
            return true;
        }
    }
}