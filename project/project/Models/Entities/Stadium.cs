namespace Project.Models.Entities;

public class Stadium
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int? ClubId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual Club? Club { get; set; }
    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}

