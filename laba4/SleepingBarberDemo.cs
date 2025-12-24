using System;
using System.Threading;

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

