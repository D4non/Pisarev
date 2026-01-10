namespace Project.Models.DTO;

public class PlayerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Position { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
}

public class CreatePlayerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Position { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
}

public class UpdatePlayerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Position { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
}

