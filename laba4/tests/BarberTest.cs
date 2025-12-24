using System;
using System.Threading;

class BarberTest
{
    public static void TestBarberShop()
    {
        Console.WriteLine("Тест парикмахерской");
        
        BarberShop shop = new BarberShop(3);
        int servedCount = 0;
        object lockObj = new object();
        
        Thread barberThread = new Thread(() => shop.BarberWork());
        barberThread.Start();
        
        Thread.Sleep(500);
        
        Thread[] customerThreads = new Thread[8];
        for (int i = 1; i <= 8; i++)
        {
            int customerId = i;
            customerThreads[i - 1] = new Thread(() =>
            {
                Thread.Sleep(new Random(customerId).Next(50, 200));
                bool served = shop.CustomerArrives(customerId);
                if (served)
                {
                    lock (lockObj)
                    {
                        servedCount++;
                    }
                }
            });
            customerThreads[i - 1].Start();
        }
        
        Thread.Sleep(10000);
        
        Console.WriteLine($"Обслужено клиентов: {servedCount}");
        
        if (servedCount > 0)
        {
            Console.WriteLine("Тест пройден - клиенты обслуживаются");
        }
        else
        {
            Console.WriteLine("Тест не пройден - клиенты не обслуживаются");
        }
        
        shop.Close();
        barberThread.Join(2000);
        
        foreach (var thread in customerThreads)
        {
            thread.Join(1000);
        }
    }
}

