## Описание задачи

Задача демонстрирует реализацию паттерна Producer/Consumer с использованием асинхронных каналов в C#. Паттерн позволяет разделить процессы производства и потребления данных, обеспечивая безопасное взаимодействие между потоками или задачами.

## Используемые технологии и библиотеки
### 1. .NET Core

- ```System.Threading.Channels``` - библиотека для асинхронных каналов

- Асинхронное программирование - async/await, Task

### 2. Пространства имен

- ```System.Threading.Channels``` - содержит классы ```Channel```, ```ChannelWriter<T>```, ```ChannelReader<T>```

- ```System.Threading.Tasks``` - для асинхронных операций

### 3. Ключевые компоненты

- ```Channel<T>``` - асинхронный канал для передачи данных

- ```ChannelWriter<T>``` - интерфейс для записи в канал

- ```ChannelReader<T>``` - интерфейс для чтения из канала


## Детали реализации
### Структура программы

```    
using System;
using System.Threading.Channels;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // 1. Создание неограниченного канала
        var channel = Channel.CreateUnbounded<int>();
        
        // 2. Запуск продюсера и консьюмера параллельно
        Task producer = ProduceAsync(channel.Writer);
        Task consumer = ConsumeAsync(channel.Reader);
        
        // 3. Ожидание завершения обеих задач
        await Task.WhenAll(producer, consumer);
    }
    
    // 4. Метод продюсера - записывает данные в канал
    static async Task ProduceAsync(ChannelWriter<int> writer)
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(200);           // Имитация работы
            await writer.WriteAsync(i);      // Отправка данных в канал
            Console.WriteLine($"Отправлено: {i}");
        }
        writer.Complete();                   // Сигнал о завершении работы
    }
    
    // 5. Метод консьюмера - читает и обрабатывает данные из канала
    static async Task ConsumeAsync(ChannelReader<int> reader)
    {
        // 6. Асинхронное чтение всех данных из канала
        await foreach (int item in reader.ReadAllAsync())
        {
            Console.WriteLine($"Обработано: {item * 10}");
        }
    }
}
```

### Создание канала

```var channel = Channel.CreateUnbounded<int>();```

- ```CreateUnbounded<T>()``` - создает канал без ограничений по количеству элементов

- Можно также использовать ```CreateBounded<T>(capacity)``` для канала с ограниченной емкостью

### ChannelWriter<T> и ChannelReader<T>

- **Writer** - используется для отправки данных в канал

- **Reader** - используется для получения данных из канала