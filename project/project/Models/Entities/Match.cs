namespace Project.Models.Entities;

public class Match
{
    public Guid Id { get; set; }
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public Guid? StadiumId { get; set; }
    public DateTime MatchDate { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public MatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual Club HomeClub { get; set; } = null!;
    public virtual Club AwayClub { get; set; } = null!;
    public virtual Stadium? Stadium { get; set; }
    public virtual ICollection<PlayerMatch> PlayerMatches { get; set; } = new List<PlayerMatch>();
}

public enum MatchStatus
{
    Scheduled = 1,
    InProgress = 2,
    Finished = 3,
    Cancelled = 4
}

