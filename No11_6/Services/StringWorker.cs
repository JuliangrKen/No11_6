namespace No11_6.Services
{
    public class StringWorker : IStringWorker
    {
        public int GetNumberChars(string str)
        {
            return str.Length;
        }

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