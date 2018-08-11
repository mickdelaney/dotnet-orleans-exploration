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
            
            return true;
        }
    }
}