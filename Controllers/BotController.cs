using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Services;
using Telegram.Bot.Types;

[ApiController]
[Route("/")]
public class BotController : ControllerBase
{
    // private TelegramBotClient bot = Bot.GetTelegramBot();

    [HttpPost]
        public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
    
    [HttpGet]
    public string Get()
    {
        return "Telegram bot was started. Bot: @udsubot Creator: @mosinisom";
    }
}
