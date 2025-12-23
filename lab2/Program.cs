namespace ConsoleApp2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Запуск бенчмарков производительности...");
        Console.WriteLine();
        
        var results = PerformanceBenchmark.RunAllBenchmarks();
        
        Console.WriteLine();
        Console.WriteLine("Итоговые результаты");
        foreach (var r in results)
        {
            Console.WriteLine($"{r.CollectionType} - {r.Operation}: {r.AverageTimeMs:F2} мс (мин: {r.MinTimeMs:F2}, макс: {r.MaxTimeMs:F2})");
        }
    }
}
