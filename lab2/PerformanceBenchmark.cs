using System.Diagnostics;
using System.Collections.Immutable;

namespace ConsoleApp2;

public class BenchmarkResult
{
    public string Operation = "";
    public string CollectionType = "";
    public double AverageTimeMs;
    public double MinTimeMs;
    public double MaxTimeMs;
}

public static class PerformanceBenchmark
{
    public const int CollectionSize = 100000;
    const int Iterations = 5;

    public static List<BenchmarkResult> RunAllBenchmarks()
    {
        var results = new List<BenchmarkResult>();
        Console.WriteLine("Размер коллекций: " + CollectionSize);
        Console.WriteLine("Итераций: " + Iterations);
        results.AddRange(BenchmarkList());
        results.AddRange(BenchmarkLinkedList());
        results.AddRange(BenchmarkQueue());
        results.AddRange(BenchmarkStack());
        results.AddRange(BenchmarkImmutableList());
        return results;
    }

    static List<BenchmarkResult> BenchmarkList()
    {
        var results = new List<BenchmarkResult>();
        Console.WriteLine("List<T>");
        results.Add(Measure("Добавление в конец", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            return list;
        }));
        results.Add(Measure("Добавление в начало", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Insert(0, i);
            return list;
        }));
        results.Add(Measure("Добавление в середину", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            int middle = CollectionSize / 2;
            for (int i = 0; i < 1000; i++)
                list.Insert(middle + i, -1);
            return list;
        }));
        results.Add(Measure("Удаление из конца", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            for (int i = 0; i < CollectionSize; i++)
                list.RemoveAt(list.Count - 1);
            return list;
        }));
        results.Add(Measure("Удаление из начала", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            for (int i = 0; i < CollectionSize; i++)
                list.RemoveAt(0);
            return list;
        }));
        results.Add(Measure("Удаление из середины", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            int middle = CollectionSize / 2;
            for (int i = 0; i < 1000; i++)
                list.RemoveAt(middle);
            return list;
        }));
        results.Add(Measure("Поиск по значению", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            int found = list.IndexOf(CollectionSize / 2);
            return list;
        }));
        results.Add(Measure("Получение по индексу", "List<T>", () =>
        {
            var list = new List<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.Add(i);
            int value = list[CollectionSize / 2];
            return list;
        }));
        return results;
    }

    static List<BenchmarkResult> BenchmarkLinkedList()
    {
        var results = new List<BenchmarkResult>();
        Console.WriteLine("LinkedList<T>");
        results.Add(Measure("Добавление в конец", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            return list;
        }));
        results.Add(Measure("Добавление в начало", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddFirst(i);
            return list;
        }));
        results.Add(Measure("Добавление в середину", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            var node = list.First!;
            for (int i = 0; i < CollectionSize / 2; i++)
                node = node.Next!;
            for (int i = 0; i < 1000; i++)
                list.AddAfter(node, -1);
            return list;
        }));
        results.Add(Measure("Удаление из конца", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            for (int i = 0; i < CollectionSize; i++)
                list.RemoveLast();
            return list;
        }));
        results.Add(Measure("Удаление из начала", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            for (int i = 0; i < CollectionSize; i++)
                list.RemoveFirst();
            return list;
        }));
        results.Add(Measure("Удаление из середины", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            var node = list.First!;
            for (int i = 0; i < CollectionSize / 2; i++)
                node = node.Next!;
            for (int i = 0; i < 1000 && node != null; i++)
            {
                var next = node.Next;
                list.Remove(node);
                node = next;
            }
            return list;
        }));
        results.Add(Measure("Поиск по значению", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            bool found = list.Contains(CollectionSize / 2);
            return list;
        }));
        results.Add(Measure("Получение по индексу", "LinkedList<T>", () =>
        {
            var list = new LinkedList<int>();
            for (int i = 0; i < CollectionSize; i++)
                list.AddLast(i);
            var node = list.First!;
            for (int i = 0; i < CollectionSize / 2 && node != null; i++)
                node = node.Next!;
            return list;
        }));
        return results;
    }

    static List<BenchmarkResult> BenchmarkQueue()
    {
        var results = new List<BenchmarkResult>();
        Console.WriteLine("Queue<T>");
        results.Add(Measure("Добавление в конец", "Queue<T>", () =>
        {
            var queue = new Queue<int>();
            for (int i = 0; i < CollectionSize; i++)
                queue.Enqueue(i);
            return queue;
        }));
        results.Add(Measure("Удаление из начала", "Queue<T>", () =>
        {
            var queue = new Queue<int>();
            for (int i = 0; i < CollectionSize; i++)
                queue.Enqueue(i);
            for (int i = 0; i < CollectionSize; i++)
                queue.Dequeue();
            return queue;
        }));
        results.Add(Measure("Поиск по значению", "Queue<T>", () =>
        {
            var queue = new Queue<int>();
            for (int i = 0; i < CollectionSize; i++)
                queue.Enqueue(i);
            bool found = queue.Contains(CollectionSize / 2);
            return queue;
        }));
        return results;
    }

    static List<BenchmarkResult> BenchmarkStack()
    {
        var results = new List<BenchmarkResult>();
        Console.WriteLine("Stack<T>");
        results.Add(Measure("Добавление в конец", "Stack<T>", () =>
        {
            var stack = new Stack<int>();
            for (int i = 0; i < CollectionSize; i++)
                stack.Push(i);
            return stack;
        }));
        results.Add(Measure("Удаление из конца", "Stack<T>", () =>
        {
            var stack = new Stack<int>();
            for (int i = 0; i < CollectionSize; i++)
                stack.Push(i);
            for (int i = 0; i < CollectionSize; i++)
                stack.Pop();
            return stack;
        }));
        results.Add(Measure("Поиск по значению", "Stack<T>", () =>
        {
            var stack = new Stack<int>();
            for (int i = 0; i < CollectionSize; i++)
                stack.Push(i);
            bool found = stack.Contains(CollectionSize / 2);
            return stack;
        }));
        return results;
    }

    static List<BenchmarkResult> BenchmarkImmutableList()
    {
        var results = new List<BenchmarkResult>();
        Console.WriteLine("ImmutableList<T>");
        results.Add(Measure("Добавление в конец", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            return list;
        }));
        results.Add(Measure("Добавление в начало", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Insert(0, i);
            return list;
        }));
        results.Add(Measure("Добавление в середину", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            int middle = CollectionSize / 2;
            for (int i = 0; i < 1000; i++)
                list = list.Insert(middle + i, -1);
            return list;
        }));
        results.Add(Measure("Удаление из конца", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            for (int i = 0; i < CollectionSize; i++)
                list = list.RemoveAt(list.Count - 1);
            return list;
        }));
        results.Add(Measure("Удаление из начала", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            for (int i = 0; i < CollectionSize; i++)
                list = list.RemoveAt(0);
            return list;
        }));
        results.Add(Measure("Удаление из середины", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            int middle = CollectionSize / 2;
            for (int i = 0; i < 1000; i++)
                list = list.RemoveAt(middle);
            return list;
        }));
        results.Add(Measure("Поиск по значению", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            int found = list.IndexOf(CollectionSize / 2);
            return list;
        }));
        results.Add(Measure("Получение по индексу", "ImmutableList<T>", () =>
        {
            var list = ImmutableList<int>.Empty;
            for (int i = 0; i < CollectionSize; i++)
                list = list.Add(i);
            int value = list[CollectionSize / 2];
            return list;
        }));
        return results;
    }

    static BenchmarkResult Measure<T>(string operationName, string collectionType, Func<T> operation)
    {
        for (int i = 0; i < 3; i++)
        {
            operation();
        }
        var times = new List<double>();
        for (int i = 0; i < Iterations; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            var sw = Stopwatch.StartNew();
            operation();
            sw.Stop();
            times.Add(sw.Elapsed.TotalMilliseconds);
        }
        double avg = 0;
        foreach (var t in times)
            avg += t;
        avg /= times.Count;
        double min = times[0];
        double max = times[0];
        foreach (var t in times)
        {
            if (t < min) min = t;
            if (t > max) max = t;
        }
        var result = new BenchmarkResult
        {
            Operation = operationName,
            CollectionType = collectionType,
            AverageTimeMs = avg,
            MinTimeMs = min,
            MaxTimeMs = max
        };
        Console.WriteLine(operationName + ": " + avg.ToString("F2") + " мс (мин: " + min.ToString("F2") + ", макс: " + max.ToString("F2") + ")");
        return result;
    }
}

