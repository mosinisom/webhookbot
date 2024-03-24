using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using System.IO;

public class Bot
{
    private static TelegramBotClient client { get; set; }
    private static string? token;

    public static TelegramBotClient GetTelegramBot()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        token = configuration.GetSection("BotConfiguration:BotToken").Value;

        if (client != null)
        {
            return client;
        }
        client = new TelegramBotClient(token);
        return client;
    }
}
