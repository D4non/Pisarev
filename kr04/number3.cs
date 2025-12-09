using System;
using System.Threading;

namespace ConsoleApplication2
{
    internal class Program
    {
        static void Main()
        {
            Thread thread1 = new Thread(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Console.Write(i + " ");
                }
            });

            Thread thread2 = new Thread(() =>
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    Console.Write(c + " ");
                }
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }
    }
}