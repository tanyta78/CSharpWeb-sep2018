namespace AsyncDemoTread
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            Thread thread = new Thread(() =>
            {
                var result = NumberOfPrimesInInterval(2, 100000);

                Console.WriteLine(result + " comes from thread #" + Thread.CurrentThread.ManagedThreadId);
            });

            thread.Start();


            for (int i = 0; i < 5; i++)
            {
                var task = Task.Run(() => NumberOfPrimesInInterval(2, 100000));
                task.ContinueWith((t) => Console.WriteLine(t.Result + " comes from thread #" + Thread.CurrentThread.ManagedThreadId));
            }

            Console.WriteLine("main " + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(new System.Diagnostics.StackTrace().ToString());
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
