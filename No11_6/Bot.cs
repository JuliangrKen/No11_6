using Microsoft.Extensions.Hosting;
using No11_6.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace No11_6
{
    /// <summary>
    /// Основной класс бота, отвечающий за подключение к Telegram API
    /// </summary>
    public class Bot : BackgroundService
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly TextMessageController textMessageController;
        private readonly InlineKeyboardController inlineKeyboardController;

        public Bot(ITelegramBotClient telegramBotClient, TextMessageController textMessageController, InlineKeyboardController inlineKeyboardController)
        {
            this.telegramBotClient = telegramBotClient;
            this.textMessageController = textMessageController;
            this.inlineKeyboardController = inlineKeyboardController;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            telegramBotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } },
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");

            return Task.FromResult(0);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            if (update.Type != UpdateType.Message)
                return;

            if (update.Message != null)
                await textMessageController.Handle(update.Message, cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}