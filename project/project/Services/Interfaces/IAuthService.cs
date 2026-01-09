using Project.Models.DTO;

namespace Project.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginDto dto);
    Task<TokenResponseDto> RegisterAsync(RegisterDto dto);
}

