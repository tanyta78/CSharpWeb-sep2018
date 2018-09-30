namespace EvenNumberThread
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            while (input == null || input.Split().Length != 2)
            {
                Console.WriteLine("Please insert two numbers divided by space.");
                input = Console.ReadLine();
            }
            var start = int.Parse(input.Split()[0]);
            var end = int.Parse(input.Split()[1]);

            if (start > end)
            {
                start = start + end;
                end = start - end;
                start = start - end;
            }

            Thread evens = new Thread(() => PrintEvenNumbers(start, end));

            evens.Start();
            evens.Join();
            Console.WriteLine("Thread finished work");
        }

        private static void PrintEvenNumbers(int start, int end)
        {
            if (start % 2 != 0)
            {
                start++;
            }
            for (int i = start; i <= end; i += 2)
            {
                Console.WriteLine(i);

            }
        }
    }
}
