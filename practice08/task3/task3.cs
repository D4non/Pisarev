using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var cts = new CancellationTokenSource();
        
        cts.CancelAfter(1000);
        
        try
        {
            await foreach (int number in GenerateNumbersAsync(cts.Token))
            {
                Console.WriteLine(number);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Отменено");
        }
    }

    static async IAsyncEnumerable<int> GenerateNumbersAsync(CancellationToken token)
    {
        int i = 0;
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(200, token);
            yield return ++i;
        }
    }
}