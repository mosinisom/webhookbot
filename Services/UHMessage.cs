using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Services;
public partial class UpdateHandlers
{
    private Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        // _botClient.SendTextMessageAsync(message.Chat.Id, "I'm a bot, I don't understand human language.");
        return Task.CompletedTask;
    }

}