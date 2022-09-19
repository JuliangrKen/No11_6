using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using No11_6.Services;
using static No11_6.Model.Session;

namespace No11_6.Controllers
{
    public class InlineKeyboardController
    {
        protected ITelegramBotClient telegramBotClient;
        protected IStorage memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            this.telegramBotClient = telegramBotClient;
            this.memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            if (callbackQuery?.Data == null)
                return;

            (FuncType funcType, string funcName) = callbackQuery.Data switch
            {
                "getsum" => (FuncType.GetSum, "получение суммы"),
                _ => (FuncType.GetNumChars, "получение количества символов в сообщении")
            };

            // Обновление пользовательской сессии новыми данными
            memoryStorage.GetSession(callbackQuery.From.Id).UserFuncType = funcType;

            // Отправляем в ответ уведомление о выборе
            await telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбранная функция - {funcName}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}