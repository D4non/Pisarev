using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Task<int>[] tasks = new Task<int>[5];
        for (int i = 0; i < 5; i++)
        {
            tasks[i] = GetDataAsync(i);
        }

        Task<int[]> whenAllTask = Task.WhenAll(tasks);

        try
        {
            int[] results = await whenAllTask;
            
            Console.WriteLine("Успешные результаты:");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"Задача {i}: {results[i]}");
            }
        }
        catch
        {
            Console.WriteLine("Ошибки:");
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].IsFaulted)
                {
                    Console.WriteLine($"Задача {i}: {tasks[i].Exception.InnerException.Message}");
                }
                else if (tasks[i].IsCompleted)
                {
                    Console.WriteLine($"Задача {i}: Успешно, результат = {tasks[i].Result}");
                }
            }
        }
    }

    static async Task<int> GetDataAsync(int taskNumber)
    {
        await Task.Delay(100);
        
        if (taskNumber % 2 == 0)
        {
            throw new Exception($"Ошибка в задаче {taskNumber}");
        }
        
        return taskNumber * 10;
    }
}