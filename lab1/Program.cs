namespace ConsoleApp2;

class Program
{
    static async Task Main(string[] args)
    {
        var serializer = new PersonSerializer();

        // Тест 1: Сериализация
        var person1 = new Person
        {
            Id = "123",
            FirstName = "Иванус",
            LastName = "Великолепный",
            Age = 18,
            Email = "i_v_anus@mail.ru",
            PhoneNumber = "89996663311",
            BirthDate = new DateTime(2007, 04, 5),
            Password = "ivanus_top"
        };
        Console.WriteLine($"{person1.FirstName} {person1.LastName}, {person1.Age} лет");
        var json = serializer.SerializeToJson(person1);
        Console.WriteLine(json);

        // Тест 2: Десериализация
        var person2 = serializer.DeserializeFromJson(json);
        Console.WriteLine($"Загружено: {person2.FirstName} {person2.LastName}");

        // Тест 3: Сохранение в файл (синхронно)
        serializer.SaveToFile(person1, "data/person1.json");
        Console.WriteLine("Сохранено");

        // Тест 4: Загрузка из файла (синхронно)
        var loaded = serializer.LoadFromFile("data/person1.json");
        Console.WriteLine($"{loaded.FirstName} {loaded.LastName}");

        // Тест 5: Сохранение в файл (асинхронно)
        await serializer.SaveToFileAsync(person1, "data/person2.json");

        // Тест 6: Загрузка из файла (асинхронно)
        var loaded2 = await serializer.LoadFromFileAsync("data/person2.json");
        Console.WriteLine(loaded2.Email);

        // Тест 7: Сохранение списка объектов в файл
        var people = new List<Person>
        {
            new Person { Id = "666", FirstName = "Артакапитек", LastName = "Обыкновенный", Age = 18, Email = "churka@yandex.ru", PhoneNumber = "89261288675", BirthDate = new DateTime(2007, 12, 16) },
            new Person { Id = "111", FirstName = "Даша", LastName = "Староста", Age = 31, Email = "starosta_sirius@talantiuspeh.ru", PhoneNumber = "89322244567", BirthDate = new DateTime(1992, 8, 25) }
        };
        serializer.SaveListToFile(people, "data/people.json");
        var loadedPeople = serializer.LoadListFromFile("data/people.json");
        Console.WriteLine($"Загружено: {loadedPeople.Count}");

        // Тест 8: FileResourceManager - запись в файл
        using (var manager = new FileResourceManager("data/test.txt", FileMode.Create))
        {
            manager.OpenForWriting();
            manager.WriteLine("Привет");
            manager.WriteLine("Мир");
        }

        // Тест 9: FileResourceManager - чтение из файла
        using (var manager = new FileResourceManager("data/test.txt", FileMode.Open))
        {
            manager.OpenForReading();
            Console.WriteLine(manager.ReadAllText());
            var info = manager.GetFileInfo();
            Console.WriteLine($"Размер файла: {info.Length}");
        }
    }
}
