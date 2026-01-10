using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Project.Models.DTO;
using Project.Services.Interfaces;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClubsController : ControllerBase
{
    private readonly IClubService _clubService;
    private readonly ILogger<ClubsController> _logger;

    public ClubsController(IClubService clubService, ILogger<ClubsController> logger)
    {
        _clubService = clubService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClubDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClubDto>>> GetAll()
    {
        var result = await _clubService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClubDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClubDto>> GetById(int id)
    {
        var result = await _clubService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ClubDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ClubDto>> Create([FromBody] CreateClubDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _clubService.CreateAsync(dto, role);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ClubDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ClubDto>> Update(int id, [FromBody] UpdateClubDto dto)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _clubService.UpdateAsync(id, dto, role);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _clubService.DeleteAsync(id, role);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

