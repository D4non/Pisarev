namespace Project.Models.DTO;

public class MatchDto
{
    public Guid Id { get; set; }
    public int HomeClubId { get; set; }
    public string HomeClubName { get; set; } = string.Empty;
    public int AwayClubId { get; set; }
    public string AwayClubName { get; set; } = string.Empty;
    public Guid? StadiumId { get; set; }
    public string? StadiumName { get; set; }
    public DateTime MatchDate { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateMatchDto
{
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public Guid? StadiumId { get; set; }
    public DateTime MatchDate { get; set; }
}

public class UpdateMatchDto
{
    public Guid? StadiumId { get; set; }
    public DateTime MatchDate { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public MatchStatusDto Status { get; set; }
}

public enum MatchStatusDto
{
    Scheduled = 1,
    InProgress = 2,
    Finished = 3,
    Cancelled = 4
}

