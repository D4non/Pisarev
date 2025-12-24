using System;
using System.Threading;

class Fork
{
    public readonly object LockObject = new object();
    public int Id { get; }

    public Fork(int id)
    {
        Id = id;
    }
}

class Philosopher
{
    private readonly int id;
    private readonly Fork leftFork;
    private readonly Fork rightFork;
    private readonly Random random;
    private int eatCount = 0;

    public Philosopher(int id, Fork leftFork, Fork rightFork)
    {
        this.id = id;
        this.leftFork = leftFork;
        this.rightFork = rightFork;
        this.random = new Random(id * 1000);
    }

    public void Think()
    {
        int time = random.Next(100, 500);
        Thread.Sleep(time);
    }

    public void Eat()
    {
        int time = random.Next(100, 300);
        Thread.Sleep(time);
        eatCount++;
    }

    public int GetEatCount() => eatCount;

    public void RunWithDeadlock()
    {
        while (true)
        {
            Think();
            
            lock (leftFork.LockObject)
            {
                Console.WriteLine($"Философ {id} взял левую вилку {leftFork.Id}");
                Thread.Sleep(50);
                lock (rightFork.LockObject)
                {
                    Console.WriteLine($"Философ {id} взял правую вилку {rightFork.Id} и начал есть");
                    Eat();
                    Console.WriteLine($"Философ {id} закончил есть");
                }
            }
            Console.WriteLine($"Философ {id} положил вилки");
        }
    }

    public void RunWithoutDeadlock()
    {
        while (true)
        {
            Think();
            
            Fork first = leftFork;
            Fork second = rightFork;
            if (leftFork.Id > rightFork.Id)
            {
                first = rightFork;
                second = leftFork;
            }
            lock (first.LockObject)
            {
                Console.WriteLine($"Философ {id} взял вилку {first.Id}");
                Thread.Sleep(50);
                lock (second.LockObject)
                {
                    Console.WriteLine($"Философ {id} взял вилку {second.Id} и начал есть");
                    Eat();
                    Console.WriteLine($"Философ {id} закончил есть");
                }
            }
            Console.WriteLine($"Философ {id} положил вилки");
        }
    }
}

class DiningPhilosophersWithDeadlock
{
    public static void Run()
    {
        Console.WriteLine("Запуск с deadlock...");
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

        Thread.Sleep(3000);
        Console.WriteLine("Демонстрация завершена");
    }
}

class DiningPhilosophersWithoutDeadlock
{
    public static void Run()
    {
        Console.WriteLine("Запуск без deadlock...");
        
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

        Thread.Sleep(5000);
        Console.WriteLine("\nРезультаты:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Философ {i} поел {philosophers[i].GetEatCount()} раз");
        }
        Console.WriteLine("Демонстрация завершена");
    }
}

