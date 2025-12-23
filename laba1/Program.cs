using ConsoleApp2.Services;

namespace ConsoleApp2;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DemoPerson();
        await DemoPersonSerializer();
        DemoFileResourceManager();
    }

    static void DemoPerson()
    {
        var person = new Person
        {
            FirstName = "Иванус",
            LastName = "Великолепный",
            Age = 18,
            Password = "ivanus_top",
            Id = "123",
            BirthDate = new DateTime(2007, 4, 5),
            Email = "i_v_anus@mail.ru",
            PhoneNumber = "89996663311"
        };
        Console.WriteLine($"Полное имя: {person.FullName}");
        Console.WriteLine($"Возраст: {person.Age}");
        Console.WriteLine($"Взрослый: {person.IsAdult}");
        Console.WriteLine($"Email: {person.Email}");
        Console.WriteLine($"Телефон: {person.PhoneNumber}");
        Console.WriteLine($"Дата рождения: {person.BirthDate:yyyy-MM-dd}");
        Console.WriteLine($"ID: {person.Id}");
        try
        {
            person.Email = "bademail";
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static async Task DemoPersonSerializer()
    {
        var serializer = new PersonSerializer();
        var person = new Person
        {
            FirstName = "Артакапитек",
            LastName = "Обыкновенный",
            Age = 18,
            Password = "churka_pass",
            Id = "666",
            BirthDate = new DateTime(2007, 12, 16),
            Email = "churka@yandex.ru",
            PhoneNumber = "89261288675"
        };
        var json = serializer.SerializeToJson(person);
        Console.WriteLine(json);
        var deserializedPerson = serializer.DeserializeFromJson(json);
        Console.WriteLine($"Имя: {deserializedPerson.FullName}");
        Console.WriteLine($"Email: {deserializedPerson.Email}");
        string filePath = "person.json";
        serializer.SaveToFile(person, filePath);
        Console.WriteLine($"Сохранено в {filePath}");
        var loadedPerson = serializer.LoadFromFile(filePath);
        Console.WriteLine($"Загружено: {loadedPerson.FullName}");
        string asyncFilePath = "person_async.json";
        await serializer.SaveToFileAsync(person, asyncFilePath);
        var asyncLoadedPerson = await serializer.LoadFromFileAsync(asyncFilePath);
        Console.WriteLine($"Асинхронно загружено: {asyncLoadedPerson.FullName}");
        var people = new List<Person>
        {
            new Person { FirstName = "Иванус", LastName = "Великолепный", Age = 18, Email = "i_v_anus@mail.ru", Id = "123", PhoneNumber = "89996663311" },
            new Person { FirstName = "Даша", LastName = "Староста", Age = 31, Email = "starosta_sirius@talantiuspeh.ru", Id = "111", PhoneNumber = "89322244567" }
        };
        string listFilePath = "people.json";
        serializer.SaveListToFile(people, listFilePath);
        Console.WriteLine($"Список сохранен в {listFilePath}");
        var loadedPeople = serializer.LoadListFromFile(listFilePath);
        Console.WriteLine($"Загружено {loadedPeople.Count} человек:");
        foreach (var p in loadedPeople)
        {
            Console.WriteLine($"{p.FullName} - {p.Email}");
        }
    }

    static void DemoFileResourceManager()
    {
        string testFilePath = "test_file.txt";
        using (var manager = new FileResourceManager(testFilePath, FileMode.Create))
        {
            manager.OpenForWriting();
            manager.WriteLine("Строка 1");
            manager.WriteLine("Строка 2");
            manager.WriteLine("Строка 3");
            Console.WriteLine("Записано 3 строки");
        }
        using (var manager = new FileResourceManager(testFilePath))
        {
            manager.OpenForReading();
            string content = manager.ReadAllText();
            Console.WriteLine("Содержимое файла:");
            Console.WriteLine(content);
            var fileInfo = manager.GetFileInfo();
            Console.WriteLine($"Размер: {fileInfo.Length} байт");
            Console.WriteLine($"Создан: {fileInfo.CreationTime:yyyy-MM-dd HH:mm:ss}");
        }
        using (var manager = new FileResourceManager(testFilePath, FileMode.Append))
        {
            manager.AppendText("Новая строка\n");
            Console.WriteLine("Текст добавлен");
        }
        using (var manager = new FileResourceManager(testFilePath))
        {
            manager.OpenForReading();
            string finalContent = manager.ReadAllText();
            Console.WriteLine("Финальное содержимое:");
            Console.WriteLine(finalContent);
        }
    }
}
