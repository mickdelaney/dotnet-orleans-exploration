﻿using System;
using System.Threading.Tasks;
using Shared;
using Orleankka;

namespace Server
{
    public class ChatUser : DispatchActorGrain, IChatUser
    {
        Task On(Join x)   => Send(x.Room, $"{Id} joined the room {x.Room} ...");
        Task On(Leave x)  => Send(x.Room, $"{Id} left the room {x.Room}!");
        Task On(Say x)    => Send(x.Room, $"{Id} said: {x.Message}");

        Task Send(string room, string message)
        {
            Console.WriteLine("[server]: " + message);

            var stream = System.StreamOf(Constants.StreamProviderNameDefault, Constants.StreamNameSpaceCustomers);

            return stream.Push(new ChatRoomMessage
            {
                User = Id,
                Text = message
            });
        }
    }
}