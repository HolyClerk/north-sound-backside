using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class Message
{
    public ChatUser Sender { get; set; }
    public ChatUser Receiver { get; set; }

    public string MessageData { get; set; }
}
