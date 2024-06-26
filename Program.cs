using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Services;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration.GetSection("BotConfiguration:BotToken").Value));
        builder.Services.AddScoped<UpdateHandlers>();
        builder.Services.AddScoped<UserInteractionController>();
        builder.Services.AddScoped<DbService>();
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")), ServiceLifetime.Singleton);


        builder.Services.AddControllers().AddNewtonsoftJson(); 

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}


