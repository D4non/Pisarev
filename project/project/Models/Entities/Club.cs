namespace Project.Models.Entities;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ClubType Type { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime Founded { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ClubPlayer> ClubPlayers { get; set; } = new List<ClubPlayer>();
    public virtual ICollection<ClubCoach> ClubCoaches { get; set; } = new List<ClubCoach>();
    public virtual Stadium? Stadium { get; set; }
    public virtual ICollection<Match> HomeMatches { get; set; } = new List<Match>();
    public virtual ICollection<Match> AwayMatches { get; set; } = new List<Match>();
}

public enum ClubType
{
    Football = 1,
    Basketball = 2
}

