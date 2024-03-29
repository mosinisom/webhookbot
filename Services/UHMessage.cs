using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Services;
public partial class UpdateHandlers
{
    private Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}, ChatId: {ChatId}, Text: {Text}", message.Type, message.Chat.Id, message.Text);
        switch (message.Type)
        {
            case MessageType.Text:
                return HandleTextMessage(message, cancellationToken);
            case MessageType.Photo:
                return HandlePhotoMessage(message, cancellationToken);
            default:
                return Task.CompletedTask;
        }
    }


    private async Task HandleTextMessage(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    private async Task HandlePhotoMessage(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
}