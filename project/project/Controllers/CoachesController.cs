using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Project.Models.DTO;
using Project.Services.Interfaces;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoachesController : ControllerBase
{
    private readonly ICoachService _coachService;
    private readonly ILogger<CoachesController> _logger;

    public CoachesController(ICoachService coachService, ILogger<CoachesController> logger)
    {
        _coachService = coachService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CoachDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CoachDto>>> GetAll()
    {
        var result = await _coachService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CoachDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CoachDto>> GetById(Guid id)
    {
        var result = await _coachService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(CoachDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CoachDto>> Create([FromBody] CreateCoachDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _coachService.CreateAsync(dto, role);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(CoachDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CoachDto>> Update(Guid id, [FromBody] UpdateCoachDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _coachService.UpdateAsync(id, dto, role);
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
        var result = await _coachService.DeleteAsync(id, role);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

