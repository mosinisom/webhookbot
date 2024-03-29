using Telegram.Bot.Types;

public class UserInteractionController
{
    enum State
    {
        Start,
        WaitingForName,
        WaitingForYear,
        WaitingFotInstitute,
        WaitingForDescription,
        WaitingForPhoto,
        Normal,
        WaitingForMessage,
        WaitingForComplaint
    }

    DbService _db;
    ILogger<UserInteractionController> _log;

    public UserInteractionController(DbService db, ILogger<UserInteractionController> log)
    {
        _db = db;
        _log = log;
    }

    public async Task Start(long chatId, string name = "No username")
    {
        await _db.AddNewUser(chatId, name);
        await SetState(chatId, (int)State.WaitingForName);
        _log.LogInformation("Start. ChatId: {chatId}", chatId);
    }

    public async Task AddName(long chatId, string name)
    {
        var user = await _db.GetUserByChatId(chatId);
        if (user == null)
        {
            _log.LogError(@"Task AddName. User not found. ChatId: {chatId}", chatId);
            return;
        }
        user.Username = name;
        await SetState(chatId, (int)State.WaitingForYear);
        _log.LogInformation("AddName. ChatId: {chatId}, name: {name}", chatId, name);
    }

    public async Task AddYear(long chatId, int year)
    {
        var user = await GetUserByChatId(chatId);
        if (user == null)
        {
            _log.LogError(@"Task AddYear. User not found. ChatId: {chatId}, year: {year}", chatId, year);
            return;
        }
        user.Year = year;
        await SetState(chatId, (int)State.WaitingFotInstitute);
        _log.LogInformation("AddYear. ChatId: {chatId}, year: {year}", chatId, year);
    }

    public async Task AddInstitute(long chatId, string institute)
    {
        var user = await GetUserByChatId(chatId);
        if (user == null)
        {
            _log.LogError(@"Task AddInstitute. User not found. ChatId: {chatId}, institute: {institute}", chatId, institute);
            return;
        }
        user.Institute = institute;
        await SetState(chatId, (int)State.WaitingForDescription);
        _log.LogInformation("AddInstitute. ChatId: {chatId}, institute: {institute}", chatId, institute);
    }

    public async Task AddDescription(long chatId, string description)
    {
        User? user = await GetUserByChatId(chatId);
        if (user == null)
        {
            _log.LogError(@"Task AddDescription. User not found. ChatId: {chatId} description: {description}", chatId, description);
            return;
        }
        user.Description = description;
        await SetState(chatId, (int)State.WaitingForPhoto);
        _log.LogInformation("AddDescription. ChatId: {chatId}, description: {description}", chatId, description);
    }

    public async Task AddPhoto(long chatId, string path)
    {
        var user = await GetUserByChatId(chatId);
        if (user == null)
        {
            _log.LogError(@"Task AddPhoto. User not found. ChatId: {chatId} path: {path}", chatId, path);
            return;
        }
        var photo = new Photo
        {
            Owner_chat_id = chatId,
            Path = path
        };
        await _db.AddPhoto(photo);
        await SetState(chatId, (int)State.Normal);
        _log.LogInformation("AddPhoto. ChatId: {chatId}, path: {path}", chatId, path);
    }

    public async Task<string?> GetPhoto(long chatId)
    {
        return await _db.GetPhoto(chatId);
    }

    public async Task SetState(long chatId, int state)
    {
        await _db.SetState(chatId, state);
    }

    public async Task<int> GetState(long chatId)
    {
        return await _db.GetState(chatId);
    }

    public async Task SendMessage(long fromChatId, long toChatId, string text)
    {
        await _db.SendMessage(fromChatId, toChatId, text);
        _log.LogInformation("SendMessage. From: {fromChatId}, to: {toChatId}, text: {text}", fromChatId, toChatId, text);
    }

    public async Task<int> GetLikesCount(long chatId)
    {
        return await _db.GetLikesCount(chatId);
    }

    public async Task LikeUser(long fromChatId, long toChatId)
    {
        await _db.LikeUser(fromChatId, toChatId);
        _log.LogInformation("LikeUser. From: {fromChatId}, to: {toChatId}", fromChatId, toChatId);
    }

    public async Task<Messages?> GetMessages(long chatId)
    {
        _log.LogInformation("GetMessages. ChatId: {chatId}", chatId);
        return await _db.GetLastMessage(chatId);
    }

    public async Task SendComplaint(long fromChatId, long toChatId, string text)
    {
        text = $"{toChatId} : {text}";
        await _db.SendMessage(fromChatId, 0, text);
        await SetState(fromChatId, (int)State.Normal);
        _log.LogInformation("Complaint sent. From: {fromChatId}, to: {toChatId}, text: {text}", fromChatId, toChatId, text);
    }

    public async Task<User?> GetUserByChatId(long chatId)
    {
        return await _db.GetUserByChatId(chatId);
    }

    public async Task<User?> RandomUser(long chatId)
    {
        User? user = await _db.GetOneRandomUser(chatId);
        int i = 0;
        while (user == null)
        {
            user = await _db.GetOneRandomUser(chatId);
            i++;
            if (i > 5)
            {
                _log.LogError("RandomUser. User not found. ChatId: {chatId}", chatId);
                return null;
            }
        }
        return user;
    }

    public async Task BanUser(long chatId, string reason = "Good reason")
    {
        await _db.BanUser(chatId, reason);
        _log.LogInformation("BanUser. ChatId: {chatId}, reason: {reason}", chatId, reason);
    }

    public async Task UnbanUser(long chatId)
    {
        await _db.UnbanUser(chatId);
        _log.LogInformation("UnbanUser. ChatId: {chatId}", chatId);
    }




}