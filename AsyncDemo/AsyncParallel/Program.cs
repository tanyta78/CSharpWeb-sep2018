namespace AsyncParallel
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            /* without ParallelFor
                * 9592
                * 00:00:03.3497569
           */
            /*with ParallelFor and  Interlocked
                *9592
                * 00:00:01.5829772
             */

            var sw = Stopwatch.StartNew();
            var result = NumberOfPrimesInInterval(2, 100000);
            Console.WriteLine(result);

            Console.WriteLine(sw.Elapsed);
        }

        public static int NumberOfPrimesInInterval(int min, int max)
        {
            int count = 0;
            Parallel.For(min, max + 1, i =>
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
                    Interlocked.Increment(ref count);
                }
            });


            return count;
        }
    }
}
