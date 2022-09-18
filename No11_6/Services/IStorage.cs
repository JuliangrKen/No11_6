using No11_6.Model;

namespace No11_6.Services
{
    public interface IStorage
    {
        /// <summary>
        /// Получение сессии пользователя по ID чата
        /// </summary>
        Session GetSession(long chatId);
    }
}