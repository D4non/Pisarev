namespace Project.Models.DTO;

public class CoachDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string LicenseType { get; set; } = string.Empty;
}

public class CreateCoachDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string LicenseType { get; set; } = string.Empty;
}

public class UpdateCoachDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string LicenseType { get; set; } = string.Empty;
}

