using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.Services;

public class DialogueService : IDialogueService
{
    private readonly ApplicationContext _context;
    private readonly IMemoryCache _memoryCache;

    public DialogueService(ApplicationContext context, 
        IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Dialogue?> GetDialogueAsync(User firstUser, User secondUser)
    {
        // Поиск и сохраненеи происходит по нахождению наименьшего айди пользователя.
        // Если у 1 пользователя айди меньше чем у второго - его айди и будет использоваться
        // в качестве ключа в кеше для диалога.
        int lowestId = firstUser.Id < secondUser.Id ? firstUser.Id : secondUser.Id;

        bool isCached = _memoryCache.TryGetValue(lowestId, out Dialogue? cachedDialogue);

        if (isCached)
            return cachedDialogue;

        Dialogue? existingDialogue = await _context.Dialogues
            .AsNoTracking()
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(dialogue =>
                (dialogue.FirstUserId == firstUser.Id && dialogue.SecondUserId == secondUser.Id) ||
                (dialogue.FirstUserId == secondUser.Id && dialogue.SecondUserId == firstUser.Id));

        if (existingDialogue is not null)
        {
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            _memoryCache.Set(lowestId, existingDialogue);
        }

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
