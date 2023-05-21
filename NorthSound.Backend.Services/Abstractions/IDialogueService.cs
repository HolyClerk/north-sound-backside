using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Backend.Services.Abstractions;

public interface IDialogueService
{
    Task<Dialogue?> GetDialogueAsync(User firstUser, User secondUser);
    Task<Dialogue> AddDialogueAsync(User firstUser, User secondUser);
    Task<Message> AddMessageAsync(MessageDTO message, Dialogue dialogue);
}
