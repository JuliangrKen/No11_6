namespace No11_6.Model
{
    /// <summary>
    /// Класс, инкапсулирующий данные о сессии пользователя
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Тип функции, которую должен исполнять бот при вводе текста пользователем
        /// </summary>
        public FuncType UserFuncType;

        /// <summary>
        /// Перечисление возможных функций бота
        /// </summary>
        public enum FuncType : byte
        {
            /// <summary>
            /// Получение суммы
            /// </summary>
            GetSum,
            /// <summary>
            /// Получение количества символов
            /// </summary>
            GetNumChars
        }
    }
}