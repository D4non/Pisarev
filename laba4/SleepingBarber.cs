using System;
using System.Collections.Generic;
using System.Threading;

class BarberShop
{
    private readonly SemaphoreSlim barberSemaphore;
    private readonly SemaphoreSlim customerSemaphore;
    private readonly Mutex queueMutex;
    private readonly Queue<int> waitingRoom;
    private readonly int maxSeats;
    private bool isOpen;

    public BarberShop(int maxSeats)
    {
        this.maxSeats = maxSeats;
        barberSemaphore = new SemaphoreSlim(0, 1);
        customerSemaphore = new SemaphoreSlim(0, maxSeats);
        queueMutex = new Mutex();
        waitingRoom = new Queue<int>();
        isOpen = true;
    }

    public void BarberWork()
    {
        Console.WriteLine("Парикмахер пришел на работу");
        while (isOpen)
        {
            if (waitingRoom.Count == 0)
            {
                Console.WriteLine("Парикмахер спит");
                barberSemaphore.Wait();
            }

            queueMutex.WaitOne();
            if (waitingRoom.Count > 0)
            {
                int customerId = waitingRoom.Dequeue();
                queueMutex.ReleaseMutex();
                Console.WriteLine($"Парикмахер стрижет клиента {customerId}");
                Thread.Sleep(new Random().Next(500, 1500));
                Console.WriteLine($"Парикмахер закончил стричь клиента {customerId}");
                customerSemaphore.Release();
            }
            else
            {
                queueMutex.ReleaseMutex();
            }
        }
        Console.WriteLine("Парикмахер ушел домой");
    }

    public bool CustomerArrives(int customerId)
    {
        queueMutex.WaitOne();
        
        if (waitingRoom.Count >= maxSeats)
        {
            queueMutex.ReleaseMutex();
            Console.WriteLine($"Клиент {customerId} ушел - нет мест");
            return false;
        }
        
        waitingRoom.Enqueue(customerId);
        Console.WriteLine($"Клиент {customerId} сел в очередь. Мест занято: {waitingRoom.Count}/{maxSeats}");
        queueMutex.ReleaseMutex();
        barberSemaphore.Release();
        customerSemaphore.Wait();
        return true;
    }

    public void Close()
    {
        isOpen = false;
        barberSemaphore.Release();
    }
}

class SleepingBarberDemo
{
    public static void Run()
    {
        BarberShop shop = new BarberShop(3);
        Thread barberThread = new Thread(() => shop.BarberWork());
        barberThread.Start();
        Thread.Sleep(500);
        Random random = new Random();
        for (int i = 1; i <= 10; i++)
        {
            int customerId = i;
            Thread customerThread = new Thread(() =>
            {
                Thread.Sleep(random.Next(100, 400));
                shop.CustomerArrives(customerId);
            });
            customerThread.Start();
        }
        
        Thread.Sleep(15000);
        shop.Close();
        barberThread.Join();
    }
}

