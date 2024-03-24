using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Services;
using Telegram.Bot.Types;

[ApiController]
[Route("/")]
public class BotController : ControllerBase
{
    private TelegramBotClient bot = Bot.GetTelegramBot();

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
        //Здесь мы пишем, что будет видно если зайти на адрес,
        //указаную в ngrok и launchSettings
        return "Telegram bot was started";
    }
}
