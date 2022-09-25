namespace No11_6.Services
{
    /// <summary>
    /// Сервис для работы со строками
    /// </summary>
    public class StringWorker : IStringWorker
    {
        /// <summary>
        /// Возвращает количество символов
        /// </summary>
        /// <param name="str">строка</param>
        /// <returns>количество символов</returns>
        public int GetNumberChars(string str)
        {
            return str.Length;
        }

        /// <summary>
        /// Возвращает сумму чисел в строке
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="separatorForSplit">символ, служащий разделителем чисел друг от друга</param>
        /// <returns>сумма чисел</returns>
        public int GetSumNumbersInString(string str, char separatorForSplit)
        {
            var numberArray = str.Split(separatorForSplit);

            var sum = 0;

            foreach (var number in numberArray)
                sum += Convert.ToInt32(number);

            return sum;
        }
    }
}