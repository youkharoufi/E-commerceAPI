using E_commerce.Models;

namespace E_commerce.Token
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
