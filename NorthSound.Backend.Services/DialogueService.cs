using NorthSound.Backend.DAL;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using System.Security.Claims;

namespace NorthSound.Backend.Services;

public class DialogueService : IDialogueService
{
    private readonly IConnectionManager _connectionManager;
    private readonly IAccountService _accountService;
    private readonly ApplicationContext _context;

    public DialogueService(
        IConnectionManager connectionManager,
        IAccountService accountService,
        ApplicationContext context)
    {
        _connectionManager = connectionManager;
        _accountService = accountService;
        _context = context;
    }

    public GenericResponse<Message> PrepareMessageForSending(MessageViewModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<GenericResponse<ChatUser>> AddChatUser(ClaimsPrincipal userClaims, string connectionId)
    {
        var usernameClaim = userClaims.Claims.FirstOrDefault(x => x.Type == "sub");
        var existingUser = await _accountService.GetUserByNameAsync(usernameClaim!.Value);

        if (existingUser is null)
            return Failed<ChatUser>("Пользователь не найден!");

        var addedChatUser = _connectionManager.AddUser(existingUser, connectionId);

        if (addedChatUser is null)
            return Failed<ChatUser>("Пользователь уже существует!");

        return Success(addedChatUser);
    }

    public void RemoveChatUser(string connectionId)
    {
        _connectionManager.RemoveUser(connectionId);
    }

    private static GenericResponse<T> Success<T>(T data)
        => GenericResponse<T>.Success(data);

    private static GenericResponse<T> Failed<T>(string details) 
        => GenericResponse<T>.Failed(details);
}

public interface IDialogueService
{
/*    GenericResponse<Message> PrepareMessageForSending(MessageViewModel model);
    GenericResponse<ChatUser> AddChatUser(ClaimsPrincipal userClaims);
    void RemoveChatUser(ClaimsPrincipal userClaims);*/
}