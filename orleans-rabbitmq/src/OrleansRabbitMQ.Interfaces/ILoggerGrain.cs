using System.Threading.Tasks;
using Orleans;

namespace OrleansRabbitMQ.Interfaces
{
    public interface ILoggerGrain : IGrainWithGuidKey
    {
        Task Log(string message);
    }
}