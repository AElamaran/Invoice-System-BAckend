using Invoice_System.DTOs.Auth;

namespace Invoice_System.Services
{
    public interface IAuthService
    {

        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
