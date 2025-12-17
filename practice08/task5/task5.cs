using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        int attempt = 0;
        var result = await RetryAsync(async () =>
        {
            attempt++;
            Console.WriteLine($"Попытка {attempt}");
            
            if (attempt < 3)
                throw new Exception("Ошибка");
                
            return "Успех";
        }, maxAttempts: 5);
        
        Console.WriteLine($"Результат: {result}");
    }

    static async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxAttempts)
    {
        int delay = 1000;
        
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch
            {
                if (attempt == maxAttempts) throw;
                
                await Task.Delay(delay);
                delay *= 2;
            }
        }
        
        throw new InvalidOperationException();
    }
}