using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await foreach (int number in GenerateNumbersAsync(5))
        {
            Console.WriteLine($"Получено: {number}");
        }
    }

    static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            await Task.Delay(200);
            yield return i;
        }
    }
}