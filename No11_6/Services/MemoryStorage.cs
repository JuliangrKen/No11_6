using No11_6.Model;
using System.Collections.Concurrent;

namespace No11_6.Services
{
    public class MemoryStorage : IStorage
    {
        /// <summary>
        /// Хранилище сессий
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> sessions = new();

        public Session GetSession(long chatId)
        {
            // Возвращаем сессию по ключу, если она существует
            if (sessions.ContainsKey(chatId))
                return sessions[chatId];

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session();
            sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}