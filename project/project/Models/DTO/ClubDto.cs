namespace Project.Models.DTO;

public class ClubDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime Founded { get; set; }
}

public class CreateClubDto
{
    public string Name { get; set; } = string.Empty;
    public ClubTypeDto Type { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime Founded { get; set; }
}

public class UpdateClubDto
{
    public string Name { get; set; } = string.Empty;
    public ClubTypeDto Type { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime Founded { get; set; }
}

public enum ClubTypeDto
{
    Football = 1,
    Basketball = 2
}

public class ClubListDto
{
    public List<ClubDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

