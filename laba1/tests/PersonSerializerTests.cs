using ConsoleApp2.Services;

namespace ConsoleApp2;

class PersonSerializerTests
{
    public static async Task RunAllTests()
    {
        var serializer = new PersonSerializer();

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

        var person2 = serializer.DeserializeFromJson(json);
        Console.WriteLine($"Загружено: {person2.FirstName} {person2.LastName}");

        serializer.SaveToFile(person1, "data/person1.json");
        Console.WriteLine("Сохранено");

        var loaded = serializer.LoadFromFile("data/person1.json");
        Console.WriteLine($"{loaded.FirstName} {loaded.LastName}");

        await serializer.SaveToFileAsync(person1, "data/person2.json");

        var loaded2 = await serializer.LoadFromFileAsync("data/person2.json");
        Console.WriteLine(loaded2.Email);

        var people = new List<Person>
        {
            new Person { Id = "666", FirstName = "Артакапитек", LastName = "Обыкновенный", Age = 18, Email = "churka@yandex.ru", PhoneNumber = "89261288675", BirthDate = new DateTime(2007, 12, 16) },
            new Person { Id = "111", FirstName = "Даша", LastName = "Староста", Age = 31, Email = "starosta_sirius@talantiuspeh.ru", PhoneNumber = "89322244567", BirthDate = new DateTime(1992, 8, 25) }
        };
        serializer.SaveListToFile(people, "data/people.json");
        var loadedPeople = serializer.LoadListFromFile("data/people.json");
        Console.WriteLine($"Загружено: {loadedPeople.Count}");
    }
}

