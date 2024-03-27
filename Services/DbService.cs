using Microsoft.EntityFrameworkCore;

public class DbService : IDisposable
{
    private readonly DataContext _context;
    public DbService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<List<User>> GetAllUsersExceptBanned()
    {
        return await _context.Users.FromSqlRaw("SELECT * FROM Users WHERE User_id NOT IN (SELECT User_id FROM Bannedusers)").ToListAsync();
    }

    public async Task<User?> GetUserByChatId(long chatId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Chat_id == chatId);
    }

    public async Task AddNewUser(long chatId, string username = "No username")
    {
        var userCheck = GetUserByChatId(chatId);
        if(userCheck != null)
            return;
        
        var user = new User
        {
            Chat_id = chatId,
            Username = username,
            Institute = "No institute",
            Year = 0,
            Description = "No description",
            LikesCount = 0
        };
        var photo = new Photo
        {
            Owner_chat_id = chatId,
            Path = "null"
        };
        _context.Users.Add(user);
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
    }

    public async Task BanUser(long chatId, string reason)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Chat_id == chatId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var bannedUser = new Banneduser
        {
            User_id = user.Id,
            Date = DateTime.Now,
            Reason = reason
        };
        _context.Bannedusers.Add(bannedUser);
        await _context.SaveChangesAsync();
    }

    public async Task UnbanUser(long chatId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Chat_id == chatId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var bannedUser = await _context.Bannedusers.FirstOrDefaultAsync(b => b.User_id == user.Id);
        if (bannedUser == null)
        {
            throw new Exception("User is not banned");
        }
        _context.Bannedusers.Remove(bannedUser);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task LikeUser(long fromChatId, long toChatId)
    {
        var like = new Like
        {
            From_chat_id = fromChatId,
            To_chat_id = toChatId,
            Date = DateTime.Now
        };
        _context.Likes.Add(like);


        await _context.SaveChangesAsync();
    }

    public async Task<int> GetLikesCount(long chatId)
    {
        return await _context.Likes.CountAsync(l => l.To_chat_id == chatId);
    }

    public async Task AddPhoto(Photo photo)
    {
        var existingPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.Owner_chat_id == photo.Owner_chat_id);
        if (existingPhoto != null)
        {
            existingPhoto.Path = photo.Path;
            _context.Photos.Update(existingPhoto);
        }
        else
        {
            _context.Photos.Add(photo);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<string?> GetPhoto(long chatId)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Owner_chat_id == chatId);
        return photo?.Path;
    }

    public async Task SetState(long chatId, int stateNum)
    {
        var state = await _context.States.FirstOrDefaultAsync(s => s.Chat_id == chatId);
        if (state == null)
        {
            state = new State
            {
                Chat_id = chatId,
                State_number = stateNum
            };
            _context.States.Add(state);
        }
        else
        {
            state.State_number = stateNum;
            _context.States.Update(state);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetState(long chatId)
    {
        var state = await _context.States.FirstOrDefaultAsync(s => s.Chat_id == chatId);
        return state?.State_number ?? 0;
    }

    public async Task SendMessage(long fromChatId, long toChatId, string text)
    {
        var message = new Messages
        {
            From_chat_id = fromChatId,
            To_chat_id = toChatId,
            Text = text,
            Date = DateTime.Now
        };
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<Messages?> GetLastMessage(long chatId)
    {
        return await _context.Messages.Where(m => m.To_chat_id == chatId).OrderByDescending(m => m.Date).FirstOrDefaultAsync();
    }

    public async Task<User?> GetOneRandomUser(long chatId)
    {
        // return await _context.Users.FromSqlRaw
        //     (@"If EXISTS (SELECT * FROM Bannedusers WHERE User_id = (SELECT User_id FROM Users WHERE Chat_id = {0}))
        //         SELECT * FROM Users WHERE Chat_id = {0}
        //         ELSE
        //         SELECT * FROM Users 
        //         WHERE User_id NOT IN (SELECT User_id FROM Bannedusers) 
        //         AND User_id NOT IN (SELECT Owner_chat_id FROM Photos WHERE Path IS NULL) 
        //         AND User_id != (SELECT User_id FROM Users WHERE Chat_id = {0}) 
        //         ORDER BY RANDOM() 
        //         LIMIT 1", chatId)
        //         .FirstOrDefaultAsync();

        return await _context.Users.FromSqlRaw
            (@" IF EXISTS (SELECT * FROM Bannedusers WHERE User_id = (SELECT User_id FROM Users WHERE Chat_id = {0}))
                SELECT * FROM Users WHERE Chat_id = {0}
                ELSE
                SELECT * FROM Users
                LEFT JOIN Bannedusers ON Users.User_id = Bannedusers.User_id
                LEFT JOIN Photos ON Users.User_id = Photos.Owner_chat_id
                WHERE Bannedusers.User_id IS NULL
                AND Photos.Path IS NOT NULL
                AND Users.User_id != (SELECT User_id FROM Users WHERE Chat_id = {0})
                ORDER BY RANDOM()
                LIMIT 1", chatId)
            .FirstOrDefaultAsync();
    }






    public void Dispose()
    {
        _context.Dispose();
    }
}