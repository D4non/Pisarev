using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories;
using Xunit;

namespace Project.Tests.Repositories;

public class PlayerRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly PlayerRepository _repository;

    public PlayerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new PlayerRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePlayer()
    {
        var player = new Player
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1995, 1, 1),
            Position = "Forward",
            JerseyNumber = 10
        };

        var result = await _repository.CreateAsync(player);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("Forward", result.Position);
        Assert.Equal(10, result.JerseyNumber);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPlayer()
    {
        var player = new Player
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1995, 1, 1),
            Position = "Forward",
            JerseyNumber = 10
        };
        await _repository.CreateAsync(player);

        var result = await _repository.GetByIdAsync(player.Id);

        Assert.NotNull(result);
        Assert.Equal(player.Id, result.Id);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPlayerNotFound()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPlayers()
    {
        var player1 = new Player
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1995, 1, 1),
            Position = "Forward",
            JerseyNumber = 10
        };
        var player2 = new Player
        {
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateTime(1996, 1, 1),
            Position = "Defender",
            JerseyNumber = 5
        };
        await _repository.CreateAsync(player1);
        await _repository.CreateAsync(player2);

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePlayer()
    {
        var player = new Player
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1995, 1, 1),
            Position = "Forward",
            JerseyNumber = 10
        };
        await _repository.CreateAsync(player);

        player.FirstName = "Updated";
        player.Position = "Midfielder";

        var result = await _repository.UpdateAsync(player);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
        Assert.Equal("Midfielder", result.Position);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePlayer()
    {
        var player = new Player
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1995, 1, 1),
            Position = "Forward",
            JerseyNumber = 10
        };
        await _repository.CreateAsync(player);

        var result = await _repository.DeleteAsync(player.Id);

        Assert.True(result);
        var deletedPlayer = await _repository.GetByIdAsync(player.Id);
        Assert.Null(deletedPlayer);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPlayerNotFound()
    {
        var result = await _repository.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

