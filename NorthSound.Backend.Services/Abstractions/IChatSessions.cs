﻿using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface IChatSessions
{
    public ChatUser? AddUser(User user, string connectionId);
    public bool RemoveUser(string connectionId);
    IEnumerable<ChatUser> GetAllConnections();   
    ChatUser? GetChatUserByUsername(string username);
    ChatUser? GetChatUserByConnectionId(string connectionId);
}