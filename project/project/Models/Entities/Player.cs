namespace Project.Models.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Position { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ClubPlayer> ClubPlayers { get; set; } = new List<ClubPlayer>();
    public virtual ICollection<PlayerMatch> PlayerMatches { get; set; } = new List<PlayerMatch>();
}

