using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class ProducerConsumerWithBlockingCollection
{
    private BlockingCollection<int> buffer;
    private int maxSize;

    public ProducerConsumerWithBlockingCollection(int maxSize)
    {
        this.maxSize = maxSize;
        buffer = new BlockingCollection<int>(maxSize);
    }

    public void Producer(int producerId, int itemsCount)
    {
        Random random = new Random(producerId * 100);
        for (int i = 0; i < itemsCount; i++)
        {
            int item = random.Next(1, 100);
            buffer.Add(item);
            Console.WriteLine($"Производитель {producerId} добавил товар {item}. Буфер: {buffer.Count}/{maxSize}");
            Thread.Sleep(random.Next(100, 300));
        }
        Console.WriteLine($"Производитель {producerId} закончил работу");
    }

    public void Consumer(int consumerId, int itemsCount)
    {
        Random random = new Random(consumerId * 200);
        for (int i = 0; i < itemsCount; i++)
        {
            int item = buffer.Take();
            Console.WriteLine($"Потребитель {consumerId} взял товар {item}. Буфер: {buffer.Count}/{maxSize}");
            Thread.Sleep(random.Next(150, 400));
        }
        Console.WriteLine($"Потребитель {consumerId} закончил работу");
    }

    public void Run()
    {
        Task producer1 = Task.Run(() => Producer(1, 10));
        Task producer2 = Task.Run(() => Producer(2, 8));
        Task consumer1 = Task.Run(() => Consumer(1, 9));
        Task consumer2 = Task.Run(() => Consumer(2, 9));

        Task.WaitAll(producer1, producer2, consumer1, consumer2);
        buffer.CompleteAdding();
    }
}

class ProducerConsumerWithSemaphore
{
    private readonly SemaphoreSlim emptySlots;
    private readonly SemaphoreSlim fullSlots;
    private readonly object lockObject = new object();
    private readonly int[] buffer;
    private int count = 0;
    private int writeIndex = 0;
    private int readIndex = 0;
    private int maxSize;

    public ProducerConsumerWithSemaphore(int maxSize)
    {
        this.maxSize = maxSize;
        buffer = new int[maxSize];
        emptySlots = new SemaphoreSlim(maxSize, maxSize);
        fullSlots = new SemaphoreSlim(0, maxSize);
    }

    public void Producer(int producerId, int itemsCount)
    {
        Random random = new Random(producerId * 100);
        for (int i = 0; i < itemsCount; i++)
        {
            int item = random.Next(1, 100);
            emptySlots.Wait();
            lock (lockObject)
            {
                buffer[writeIndex] = item;
                writeIndex = (writeIndex + 1) % maxSize;
                count++;
                Console.WriteLine($"Производитель {producerId} добавил товар {item}. Буфер: {count}/{maxSize}");
            }
            fullSlots.Release();
            Thread.Sleep(random.Next(100, 300));
        }
        Console.WriteLine($"Производитель {producerId} закончил работу");
    }

    public void Consumer(int consumerId, int itemsCount)
    {
        Random random = new Random(consumerId * 200);
        for (int i = 0; i < itemsCount; i++)
        {
            fullSlots.Wait();
            int item;
            lock (lockObject)
            {
                item = buffer[readIndex];
                readIndex = (readIndex + 1) % maxSize;
                count--;
                Console.WriteLine($"Потребитель {consumerId} взял товар {item}. Буфер: {count}/{maxSize}");
            }
            emptySlots.Release();
            Thread.Sleep(random.Next(150, 400));
        }
        Console.WriteLine($"Потребитель {consumerId} закончил работу");
    }

    public void Run()
    {
        Task producer1 = Task.Run(() => Producer(1, 10));
        Task producer2 = Task.Run(() => Producer(2, 8));
        Task consumer1 = Task.Run(() => Consumer(1, 9));
        Task consumer2 = Task.Run(() => Consumer(2, 9));

        Task.WaitAll(producer1, producer2, consumer1, consumer2);
    }
}

class ProducerConsumerDemo
{
    public static void RunBlockingCollection()
    {
        Console.WriteLine("Производитель-Потребитель с BlockingCollection:");
        ProducerConsumerWithBlockingCollection pc = new ProducerConsumerWithBlockingCollection(5);
        pc.Run();
    }

    public static void RunSemaphore()
    {
        Console.WriteLine("\nПроизводитель-Потребитель с SemaphoreSlim:");
        ProducerConsumerWithSemaphore pc = new ProducerConsumerWithSemaphore(5);
        pc.Run();
    }
}

