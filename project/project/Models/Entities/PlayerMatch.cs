namespace Project.Models.Entities;

public class PlayerMatch
{
    public Guid PlayerId { get; set; }
    public Guid MatchId { get; set; }
    public int? Points { get; set; }
    public int? Assists { get; set; }
    public int? Goals { get; set; }
    public int? Rebounds { get; set; }
    public int MinutesPlayed { get; set; }

    public virtual Player Player { get; set; } = null!;
    public virtual Match Match { get; set; } = null!;
}

