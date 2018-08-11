using System;
using Orleankka;

namespace Shared
{
    [Serializable]
    public class Leave : ActorMessage<IChatUser>
    {
        public string Room;
    }
}