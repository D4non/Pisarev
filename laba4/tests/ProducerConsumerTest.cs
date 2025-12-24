using System;
using System.Threading;
using System.Threading.Tasks;

class ProducerConsumerTest
{
    public static void TestBlockingCollection()
    {
        Console.WriteLine("Тест Producer-Consumer с BlockingCollection");
        
        ProducerConsumerWithBlockingCollection pc = new ProducerConsumerWithBlockingCollection(5);
        
        Task producer1 = Task.Run(() => pc.Producer(1, 5));
        Task producer2 = Task.Run(() => pc.Producer(2, 5));
        Task consumer1 = Task.Run(() => pc.Consumer(1, 5));
        Task consumer2 = Task.Run(() => pc.Consumer(2, 5));

        Task.WaitAll(producer1, producer2, consumer1, consumer2);
        
        Console.WriteLine("Тест завершен - все задачи выполнены");
    }

    public static void TestSemaphore()
    {
        Console.WriteLine("Тест Producer-Consumer с SemaphoreSlim");
        
        ProducerConsumerWithSemaphore pc = new ProducerConsumerWithSemaphore(5);
        
        Task producer1 = Task.Run(() => pc.Producer(1, 5));
        Task producer2 = Task.Run(() => pc.Producer(2, 5));
        Task consumer1 = Task.Run(() => pc.Consumer(1, 5));
        Task consumer2 = Task.Run(() => pc.Consumer(2, 5));

        Task.WaitAll(producer1, producer2, consumer1, consumer2);

    }
}

