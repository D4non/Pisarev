using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp2;

public class PersonSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public string SerializeToJson(Person person)
    {
        return JsonSerializer.Serialize(person, Options);
    }

    public Person DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<Person>(json, Options) 
               ?? throw new InvalidOperationException("Не удалось десериализовать объект");
    }

    public void SaveToFile(Person person, string filePath)
    {
        var json = SerializeToJson(person);
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    public Person LoadFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath, Encoding.UTF8);
        return DeserializeFromJson(json);
    }

    public async Task SaveToFileAsync(Person person, string filePath)
    {
        var json = SerializeToJson(person);
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
    }

    public async Task<Person> LoadFromFileAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        return DeserializeFromJson(json);
    }

    public void SaveListToFile(List<Person> people, string filePath)
    {
        var json = JsonSerializer.Serialize(people, Options);
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    public List<Person> LoadListFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<List<Person>>(json, Options) ?? new List<Person>();
    }
}

