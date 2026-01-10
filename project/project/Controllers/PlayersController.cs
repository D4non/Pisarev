using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Project.Models.DTO;
using Project.Services.Interfaces;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
    {
        _playerService = playerService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlayerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll()
    {
        var result = await _playerService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerDto>> GetById(Guid id)
    {
        var result = await _playerService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PlayerDto>> Create([FromBody] CreatePlayerDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _playerService.CreateAsync(dto, role);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PlayerDto>> Update(Guid id, [FromBody] UpdatePlayerDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _playerService.UpdateAsync(id, dto, role);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _playerService.DeleteAsync(id, role);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

