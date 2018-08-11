using System;
using Orleankka;

namespace Shared
{
    [Serializable]
    public class Join : ActorMessage<IChatUser>
    {
        public string Room;
    }
}