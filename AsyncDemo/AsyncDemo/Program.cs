namespace AsyncDemoTask
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            PrintCount();

            while (true)
            {
                string line = Console.ReadLine();
                if (line == "exit")
                {
                    return;
                }
                else
                {
                    Console.WriteLine(line);
                }
            }

        }

        public static async void PrintCount()
        {
            try
            {
                var result = await NumberOfPrimesInIntervalAsync(2, 100000);
                Console.WriteLine(result);
                Console.WriteLine(DateTime.Now);
                Thread.Sleep(10000);
                Console.WriteLine(DateTime.Now);
            }
            catch (Exception e)
            {
                Console.WriteLine("!!!!!!!!!!!!!!");
             
            }
        }

        public static Task<int> NumberOfPrimesInIntervalAsync(int min, int max)
        {
           // throw new Exception("Something went wrong!");
            return Task.Run(() => NumberOfPrimesInInterval(min, max));
        }

        public static int NumberOfPrimesInInterval(int min, int max)
        {
            int count = 0;
            for (int i = min; i <= max; i++)
            {
                bool isPrime = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
