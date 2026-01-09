namespace Project.Models.Entities;

public class ClubCoach
{
    public int ClubId { get; set; }
    public Guid CoachId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime? EndedDate { get; set; }
    public bool IsActive { get; set; }

    public virtual Club Club { get; set; } = null!;
    public virtual Coach Coach { get; set; } = null!;
}

