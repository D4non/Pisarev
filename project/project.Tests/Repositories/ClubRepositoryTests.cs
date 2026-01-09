using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using Project.Models.Entities;
using Project.Repositories;
using Xunit;

namespace Project.Tests.Repositories;

public class ClubRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ClubRepository _repository;
    private readonly IDistributedCache _cache;

    public ClubRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        _cache = serviceProvider.GetRequiredService<IDistributedCache>();

        _repository = new ClubRepository(_context, _cache);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateClub()
    {
        var club = new Club
        {
            Name = "Test Club",
            Type = ClubType.Football,
            City = "Test City",
            Country = "Test Country",
            Founded = new DateTime(2020, 1, 1)
        };

        var result = await _repository.CreateAsync(club);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Club", result.Name);
        Assert.Equal(ClubType.Football, result.Type);
        Assert.Equal("Test City", result.City);
        Assert.Equal("Test Country", result.Country);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnClub()
    {
        var club = new Club
        {
            Name = "Test Club",
            Type = ClubType.Football,
            City = "Test City",
            Country = "Test Country",
            Founded = new DateTime(2020, 1, 1)
        };
        await _repository.CreateAsync(club);

        var result = await _repository.GetByIdAsync(club.Id);

        Assert.NotNull(result);
        Assert.Equal(club.Id, result.Id);
        Assert.Equal("Test Club", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenClubNotFound()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllClubs()
    {
        var club1 = new Club
        {
            Name = "Club 1",
            Type = ClubType.Football,
            City = "City 1",
            Country = "Country 1",
            Founded = new DateTime(2020, 1, 1)
        };
        var club2 = new Club
        {
            Name = "Club 2",
            Type = ClubType.Basketball,
            City = "City 2",
            Country = "Country 2",
            Founded = new DateTime(2021, 1, 1)
        };
        await _repository.CreateAsync(club1);
        await _repository.CreateAsync(club2);

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateClub()
    {
        var club = new Club
        {
            Name = "Original Name",
            Type = ClubType.Football,
            City = "Original City",
            Country = "Original Country",
            Founded = new DateTime(2020, 1, 1)
        };
        await _repository.CreateAsync(club);

        club.Name = "Updated Name";
        club.City = "Updated City";

        var result = await _repository.UpdateAsync(club);

        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal("Updated City", result.City);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteClub()
    {
        var club = new Club
        {
            Name = "Test Club",
            Type = ClubType.Football,
            City = "Test City",
            Country = "Test Country",
            Founded = new DateTime(2020, 1, 1)
        };
        await _repository.CreateAsync(club);

        var result = await _repository.DeleteAsync(club.Id);

        Assert.True(result);
        var deletedClub = await _repository.GetByIdAsync(club.Id);
        Assert.Null(deletedClub);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenClubNotFound()
    {
        var result = await _repository.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResults()
    {
        for (int i = 1; i <= 15; i++)
        {
            var club = new Club
            {
                Name = $"Club {i}",
                Type = ClubType.Football,
                City = "City",
                Country = "Country",
                Founded = new DateTime(2020, 1, 1)
            };
            await _repository.CreateAsync(club);
        }

        var (items, total) = await _repository.GetPagedAsync(1, 10, null);

        Assert.Equal(15, total);
        Assert.Equal(10, items.Count());
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterBySearch()
    {
        var club1 = new Club { Name = "Football Club", Type = ClubType.Football, City = "Moscow", Country = "Russia", Founded = new DateTime(2020, 1, 1) };
        var club2 = new Club { Name = "Basketball Club", Type = ClubType.Basketball, City = "Moscow", Country = "Russia", Founded = new DateTime(2020, 1, 1) };
        var club3 = new Club { Name = "Other Club", Type = ClubType.Football, City = "SPB", Country = "Russia", Founded = new DateTime(2020, 1, 1) };
        await _repository.CreateAsync(club1);
        await _repository.CreateAsync(club2);
        await _repository.CreateAsync(club3);

        var (items, total) = await _repository.GetPagedAsync(1, 10, "Football");

        Assert.Equal(1, total);
        Assert.Single(items);
        Assert.Equal("Football Club", items.First().Name);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

