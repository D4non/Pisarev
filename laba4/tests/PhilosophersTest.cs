using System;
using System.Threading;
using System.Diagnostics;

class PhilosophersTest
{
    public static void TestDeadlock()
    {
        Console.WriteLine("Тест deadlock - должен зависнуть через несколько секунд");
        
        Fork[] forks = new Fork[5];
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new Fork(i);
        }

        Philosopher[] philosophers = new Philosopher[5];
        for (int i = 0; i < 5; i++)
        {
            philosophers[i] = new Philosopher(i, forks[i], forks[(i + 1) % 5]);
        }

        Thread[] threads = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            int index = i;
            threads[i] = new Thread(() => philosophers[index].RunWithDeadlock());
            threads[i].Start();
        }

        Stopwatch sw = Stopwatch.StartNew();
        Thread.Sleep(2000);
        
        bool deadlocked = sw.ElapsedMilliseconds >= 2000 && 
                         threads[0].ThreadState == System.Threading.ThreadState.WaitSleepJoin;
        
        Console.WriteLine($"Прошло {sw.ElapsedMilliseconds} мс");
        Console.WriteLine(deadlocked ? "Deadlock обнаружен" : "Deadlock не обнаружен");
        Console.WriteLine("Тест завершен");
    }

    public static void TestNoDeadlock()
    {
        Console.WriteLine("Тест без deadlock - должен работать корректно");
        
        Fork[] forks = new Fork[5];
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new Fork(i);
        }

        Philosopher[] philosophers = new Philosopher[5];
        for (int i = 0; i < 5; i++)
        {
            philosophers[i] = new Philosopher(i, forks[i], forks[(i + 1) % 5]);
        }

        Thread[] threads = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            int index = i;
            threads[i] = new Thread(() => philosophers[index].RunWithoutDeadlock());
            threads[i].Start();
        }

        Thread.Sleep(3000);
        
        bool allAte = true;
        for (int i = 0; i < 5; i++)
        {
            int count = philosophers[i].GetEatCount();
            Console.WriteLine($"Философ {i} поел {count} раз");
            if (count == 0) allAte = false;
        }
        
        Console.WriteLine(allAte ? "Тест пройден - все философы ели" : "Тест не пройден");
    }
}

