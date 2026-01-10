using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;

namespace Project.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Club> Clubs { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<ClubPlayer> ClubPlayers { get; set; }
    public DbSet<ClubCoach> ClubCoaches { get; set; }
    public DbSet<PlayerMatch> PlayerMatches { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("clubs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Type).HasColumnName("type").IsRequired();
            entity.Property(e => e.City).HasColumnName("city").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).HasColumnName("country").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Founded).HasColumnName("founded").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("players");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
            entity.Property(e => e.Position).HasColumnName("position").IsRequired().HasMaxLength(50);
            entity.Property(e => e.JerseyNumber).HasColumnName("jersey_number").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.ToTable("coaches");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
            entity.Property(e => e.LicenseType).HasColumnName("license_type").IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("matches");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HomeClubId).HasColumnName("home_club_id").IsRequired();
            entity.Property(e => e.AwayClubId).HasColumnName("away_club_id").IsRequired();
            entity.Property(e => e.StadiumId).HasColumnName("stadium_id");
            entity.Property(e => e.MatchDate).HasColumnName("match_date").IsRequired();
            entity.Property(e => e.HomeScore).HasColumnName("home_score");
            entity.Property(e => e.AwayScore).HasColumnName("away_score");
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.HomeClub)
                .WithMany(c => c.HomeMatches)
                .HasForeignKey(e => e.HomeClubId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.AwayClub)
                .WithMany(c => c.AwayMatches)
                .HasForeignKey(e => e.AwayClubId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Stadium)
                .WithMany(s => s.Matches)
                .HasForeignKey(e => e.StadiumId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Stadium>(entity =>
        {
            entity.ToTable("stadiums");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).HasColumnName("city").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Capacity).HasColumnName("capacity").IsRequired();
            entity.Property(e => e.ClubId).HasColumnName("club_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Club)
                .WithOne(c => c.Stadium)
                .HasForeignKey<Stadium>(e => e.ClubId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ClubPlayer>(entity =>
        {
            entity.ToTable("club_players");
            entity.HasKey(e => new { e.ClubId, e.PlayerId });
            entity.HasOne(e => e.Club)
                .WithMany(c => c.ClubPlayers)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Player)
                .WithMany(p => p.ClubPlayers)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ClubCoach>(entity =>
        {
            entity.ToTable("club_coaches");
            entity.HasKey(e => new { e.ClubId, e.CoachId });
            entity.HasOne(e => e.Club)
                .WithMany(c => c.ClubCoaches)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Coach)
                .WithMany(co => co.ClubCoaches)
                .HasForeignKey(e => e.CoachId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlayerMatch>(entity =>
        {
            entity.ToTable("player_matches");
            entity.HasKey(e => new { e.PlayerId, e.MatchId });
            entity.HasOne(e => e.Player)
                .WithMany(p => p.PlayerMatches)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Match)
                .WithMany(m => m.PlayerMatches)
                .HasForeignKey(e => e.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.ToTable("api_keys");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).HasColumnName("key").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.HasIndex(e => e.Key).IsUnique();
        });
    }
}

