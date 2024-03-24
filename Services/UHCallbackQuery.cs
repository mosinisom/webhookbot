using Telegram.Bot.Types;

namespace Telegram.Bot.Services;
public partial class UpdateHandlers
{
    private Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);
        return Task.CompletedTask;
    }

}