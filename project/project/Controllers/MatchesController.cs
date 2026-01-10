using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Project.Models.DTO;
using Project.Services.Interfaces;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly ILogger<MatchesController> _logger;

    public MatchesController(IMatchService matchService, ILogger<MatchesController> logger)
    {
        _matchService = matchService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
    {
        var result = await _matchService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatchDto>> GetById(Guid id)
    {
        var result = await _matchService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<MatchDto>> Create([FromBody] CreateMatchDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _matchService.CreateAsync(dto, role);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<MatchDto>> Update(Guid id, [FromBody] UpdateMatchDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _matchService.UpdateAsync(id, dto, role);
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
        var result = await _matchService.DeleteAsync(id, role);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

