using AutoBid.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoBid.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context; // Replace YourDbContext with your actual DbContext class

        public UserService(DataContext context)
        {
            _context = context;
        }

        public Task GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserIdAsync(string userEmail)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
            return user?.Id; // Adjust this to match your User model's ID property
        }
        
    }
}
