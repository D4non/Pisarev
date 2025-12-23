using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Лабораторная работа 4");
        Console.WriteLine("Выберите задачу:");
        Console.WriteLine("1 - Обедающие философы (с deadlock)");
        Console.WriteLine("2 - Обедающие философы (без deadlock)");
        Console.WriteLine("3 - Спящий парикмахер");
        Console.WriteLine("4 - Производитель-Потребитель (BlockingCollection)");
        Console.WriteLine("5 - Производитель-Потребитель (SemaphoreSlim)");
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                DiningPhilosophersWithDeadlock.Run();
                break;
            case "2":
                DiningPhilosophersWithoutDeadlock.Run();
                break;
            case "3":
                SleepingBarberDemo.Run();
                break;
            case "4":
                ProducerConsumerDemo.RunBlockingCollection();
                break;
            case "5":
                ProducerConsumerDemo.RunSemaphore();
                break;
            default:
                Console.WriteLine("Неверный выбор");
                break;
        }
    }
}

