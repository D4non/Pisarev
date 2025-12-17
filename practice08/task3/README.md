## Описание задачи

Задача демонстрирует реализацию отменяемого асинхронного потока данных с использованием ```CancellationToken``` и ```IAsyncEnumerable<T>```.

## Используемые технологии и библиотеки
### 1. C# 8.0+

- Асинхронные потоки - ```IAsyncEnumerable<T>```

- Механизмы отмены - ```CancellationToken```, ```CancellationTokenSource```

### 2. Пространства имен

- ```System.Collections.Generic``` - ```IAsyncEnumerable<T>```

- ```System.Threading``` - ```CancellationToken```, ```CancellationTokenSource```

- ```System.Threading.Tasks``` - асинхронные операции


## Детали реализации
### Структура программы

```    
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // 1. Создание источника отмены
        var cts = new CancellationTokenSource();
        
        // 2. Автоматическая отмена через 1 секунду
        cts.CancelAfter(1000);
        
        try
        {
            // 3. Асинхронный перебор с токеном отмены
            await foreach (int number in GenerateNumbersAsync(cts.Token))
            {
                Console.WriteLine(number);
            }
        }
        catch (OperationCanceledException)
        {
            // 4. Обработка отмены
            Console.WriteLine("Отменено");
        }
    }

    // 5. Асинхронный генератор с поддержкой отмены
    static async IAsyncEnumerable<int> GenerateNumbersAsync(CancellationToken token)
    {
        int i = 0;
        while (!token.IsCancellationRequested)
        {
            // 6. Задержка с проверкой отмены
            await Task.Delay(200, token);
            yield return ++i;
        }
    }
}
```

### ```CancellationTokenSource``` и ```CancellationToken```

- ```CancellationTokenSource``` - создает и управляет токенами отмены

- ```CancellationToken``` - распространяет уведомление об отмене

### Автоматическая отмена

```cts.CancelAfter(1000); // Отмена через 1000 мс```

- Удобный способ отложенной отмены

### Проверка отмены в цикле

```while (!token.IsCancellationRequested)```

- Регулярная проверка состояния отмены

- Позволяет корректно завершить цикл при отмене