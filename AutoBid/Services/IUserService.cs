using AutoBid.Dtos.User;

namespace AutoBid.Services
{
    public interface IUserService
    {
        Task GetUserByIdAsync(int id);
        Task<string> GetUserIdAsync(string userEmail);
    }
}
