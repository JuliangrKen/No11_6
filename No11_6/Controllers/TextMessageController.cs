using No11_6.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static No11_6.Model.Session;

namespace No11_6.Controllers
{
    public class TextMessageController
    {
        protected ITelegramBotClient telegramBotClient;
        protected IStorage memoryStorage;
        protected IStringWorker stringWorker;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, IStringWorker stringWorker)
        {
            this.telegramBotClient = telegramBotClient;
            this.memoryStorage = memoryStorage;
            this.stringWorker = stringWorker;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} среагировал на сообщение");

            // Проверка на null на команды
            switch (message.Text)
            {
                case null: return;
                case "/start":

                    var buttons = new List<InlineKeyboardButton[]>
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData($"Получать сумму чисел" , $"getsum"),
                            InlineKeyboardButton.WithCallbackData($"Получать количество символов" , $"getnumchars")
                        }
                    };

                    await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"<b>Выбирите необходимую функцию.</b>", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    return;
            }

            // Если не команды, то срабатывает один из вариантов функционала
            switch (memoryStorage.GetSession(message.Chat.Id).UserFuncType)
            {
                case FuncType.GetSum:
                    try
                    {
                        var sum = stringWorker.GetSumNumbersInString(message.Text, ' ');
                        await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел равна {sum}", cancellationToken: ct);
                    }
                    catch (FormatException)
                    {
                        await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Ошибка, посторонние символы", cancellationToken: ct);
                    }
                    catch (OverflowException)
                    {
                        await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Ошибка, слишком большие числа", cancellationToken: ct);
                    }
                    break;
                default:
                    await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Количество символов в сообщении: {stringWorker.GetNumberChars(message.Text)}", cancellationToken: ct);
                    break;
            }
        }
    }
}