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

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            this.telegramBotClient = telegramBotClient;
            this.memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            // Проверка на команды
            switch (message.Text)
            {
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
            switch(memoryStorage.GetSession(message.Chat.Id).UserFuncType)
            {
                case FuncType.GetSum:
                    await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Выполнена FuncType.GetSum", cancellationToken: ct);
                    break;
                default:
                    await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Выполнена FuncType.GetSumChars", cancellationToken: ct);
                    break;
            }
        }
    }
}