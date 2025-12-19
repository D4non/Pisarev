namespace ConsoleApp2;

class Program
{
    static void Main(string[] args)
    {
        var results = PerformanceBenchmark.RunAllBenchmarks();
        Console.WriteLine();
        foreach (var r in results)
        {
            Console.WriteLine(r.CollectionType + " - " + r.Operation + ": " + r.AverageTimeMs.ToString("F2") + " мс");
        }
    }
}
