# Лабораторная работа 4

Изучение проблемы синхронизации потоков, выявление и предотвращение взаимоблокировок (deadlock).

## Задачи

### 1. Обедающие философы (Dining Philosophers)

Реализованы две версии:
- С deadlock - демонстрирует проблему взаимоблокировки
- Без deadlock - исправленная версия с упорядочиванием вилок по ID

Используется: `lock`

### 2. Спящий парикмахер (Sleeping Barber)

Моделирование работы парикмахерской с ограниченной очередью.

Используется: `SemaphoreSlim`, `Mutex`, `Queue<int>`

### 3. Производитель-Потребитель (Producer-Consumer)

Две реализации:
- С использованием `BlockingCollection`
- С использованием `SemaphoreSlim` + `lock`

## Структура проекта

```
laba4/
├── DiningPhilosophers.cs
├── SleepingBarber.cs
├── ProducerConsumer.cs
├── tests/
│   ├── PhilosophersTest.cs
│   ├── BarberTest.cs
│   └── ProducerConsumerTest.cs
├── Program.cs
└── laba4.csproj
```

## Запуск

### Основная программа

```bash
dotnet run
```

Выберите задачу:
1. Обедающие философы (с deadlock)
2. Обедающие философы (без deadlock)
3. Спящий парикмахер
4. Производитель-Потребитель (BlockingCollection)
5. Производитель-Потребитель (SemaphoreSlim)

### Тесты

Тесты находятся в папке `tests/` и могут быть запущены отдельно.


