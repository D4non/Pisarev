using System.Text.Json.Serialization;

namespace ConsoleApp2;

public class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }

    public string Email
    {
        get => _email;
        set
        {
            if (!value.Contains('@'))
                throw new ArgumentException("Email должен содержать символ '@'");
            _email = value;
        }
    }
    private string _email = string.Empty;

    [JsonIgnore]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("personId")]
    public string Id { get; set; } = string.Empty;

    [JsonInclude]
    private DateTime _birthDate;

    [JsonIgnore]
    public DateTime BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    [JsonPropertyName("phone")]
    public string PhoneNumber { get; set; } = string.Empty;
}

