using System;
using Orleankka;

namespace Shared
{
    [Serializable]
    public class Say : ActorMessage<IChatUser>
    {
        public string Room;
        public string Message;
    }
}