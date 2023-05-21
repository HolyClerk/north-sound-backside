using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.Services;

public class DialogueService : IDialogueService
{
    private readonly ApplicationContext _context;

    public DialogueService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Dialogue?> GetDialogueAsync(User firstUser, User secondUser)
    {
        Dialogue? existingDialogue = await _context.Dialogues
            .AsNoTracking()
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(dialogue =>
                (dialogue.FirstUserId == firstUser.Id && dialogue.SecondUserId == secondUser.Id) ||
                (dialogue.FirstUserId == secondUser.Id && dialogue.SecondUserId == firstUser.Id));

        return existingDialogue;
    }

    public async Task<Dialogue> AddDialogueAsync(User firstUser, User secondUser)
    {
        var newDialogue = new Dialogue
        {
            FirstUserId = firstUser.Id,
            SecondUserId = secondUser.Id,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Dialogues.AddAsync(newDialogue);
        await _context.SaveChangesAsync();
        return newDialogue;
    }

    public async Task<Message> AddMessageAsync(MessageDTO messageDTO, Dialogue dialogue)
    {
        var message = new Message
        {
            SenderId = messageDTO.Sender.Id,
            ReceiverId = messageDTO.Receiver.Id,
            Text = messageDTO.Value,
            DialogueId = dialogue.Id,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        await _context.Entry(message).Reference(m => m.Sender).LoadAsync();
        await _context.Entry(message).Reference(m => m.Receiver).LoadAsync();

        return message;
    }
}
