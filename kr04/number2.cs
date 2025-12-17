using System;

namespace ConsoleApplication2
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("SOH:");
        
            for (int i = 0; i < 5; i++)
            {
                byte[] small = new byte[1000];
                Console.WriteLine($"Объект {i+1}: поколение {GC.GetGeneration(small)}");
            }
        
            Console.WriteLine("LOH:");
        
            for (int i = 0; i < 5; i++)
            {
                byte[] large = new byte[100000];
                Console.WriteLine($"Объект {i+1}: поколение {GC.GetGeneration(large)}");
            }
        }
    }    
}
