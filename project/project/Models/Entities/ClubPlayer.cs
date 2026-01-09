namespace Project.Models.Entities;

public class ClubPlayer
{
    public int ClubId { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime? LeftDate { get; set; }
    public bool IsActive { get; set; }

    public virtual Club Club { get; set; } = null!;
    public virtual Player Player { get; set; } = null!;
}

