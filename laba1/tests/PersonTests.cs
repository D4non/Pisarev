using ConsoleApp2.Services;

namespace ConsoleApp2;

class PersonTests
{
    public static void RunAllTests()
    {
        Console.WriteLine("=== Тесты класса Person ===");

        var person = new Person
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
        Console.WriteLine($"Создан объект: {person.FirstName} {person.LastName}, {person.Age} лет");
        Console.WriteLine($"Email: {person.Email}");
        Console.WriteLine($"Phone: {person.PhoneNumber}");
        Console.WriteLine($"BirthDate: {person.BirthDate:yyyy-MM-dd}");
        Console.WriteLine($"Password (не сериализуется): {person.Password}");
        
        try
        {
            person.Email = "test@example.com";
            Console.WriteLine($"✓ Корректный email установлен: {person.Email}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"✗ Ошибка: {ex.Message}");
        }
        
        try
        {
            person.Email = "invalid-email";
            Console.WriteLine($"✗ Ошибка: Email без '@' был принят");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"✓ Валидация работает: {ex.Message}");
        }
        
        Console.WriteLine($"Id (JsonPropertyName 'personId'): {person.Id}");
        Console.WriteLine($"PhoneNumber (JsonPropertyName 'phone'): {person.PhoneNumber}");
        
        Console.WriteLine("=== Тесты Person завершены ===\n");
    }
}

