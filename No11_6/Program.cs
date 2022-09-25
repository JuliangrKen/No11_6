using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using No11_6;
using No11_6.Configuration;
using No11_6.Controllers;
using No11_6.Services;
using System.Text.Json;
using Telegram.Bot;

var host = new HostBuilder()
    .ConfigureServices((hostContext, services) => ConfigureServices(services))
    .UseConsoleLifetime()
    .Build();

Console.WriteLine("Сервис запущен");
await host.RunAsync();
Console.WriteLine("Сервис остановлен");

void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<InlineKeyboardController>();
    services.AddTransient<TextMessageController>();

    // Регистрируем метод получения конфигурации бота
    services.AddSingleton(GetBotConfig());
    var botConfig = GetBotConfig();
    // Регистрируем объект TelegramBotClient c токеном подключения
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(botConfig.Token ?? ""));
    // Регистрируем постоянно активный сервис бота
    services.AddHostedService<Bot>();
    // Регистрируем сервис получения данных о сессии пользователя
    services.AddSingleton<IStorage, MemoryStorage>();
    // Регистрируем сервис для работы со строками
    services.AddSingleton<IStringWorker, StringWorker>();
}

BotConfig GetBotConfig()
{
    var json = File.ReadAllText($@"{Environment.CurrentDirectory}/Configuration/BotConfig.json"); // Достаём из конфига объект 
    return JsonSerializer.Deserialize<BotConfig>(json) // Десериализуем его в BotConfig
           ?? throw new ArgumentNullException(); // При неудаче вызываем ошибку нулевого аргумента
}