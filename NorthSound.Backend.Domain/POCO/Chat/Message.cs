using NorthSound.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class Message
{
    public Message() { }

    public Message(UserDTO sender, UserDTO receiver, string messageData)
    {
        Sender = sender;
        Receiver = receiver;
        Value = messageData;
    }

    public UserDTO Sender { get; set; }
    public UserDTO Receiver { get; set; }

    public string Value { get; set; }
}
