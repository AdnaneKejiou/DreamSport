using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Models;
using gestionUtilisateur.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace gestionUtilisateur.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context; // Replace with your DbContext name

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AddUserManualyAsync(User _user)
        {
            try
            {
                var result = await _context.Users.AddAsync(_user);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<bool> DoesUserWithEmailExist(string email, int id)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.IdAdmin == id);
        }

        public async Task<bool> DoesUserWithPhoneExist(string phone, int id)
        {
            return await _context.Users.AnyAsync(u => u.Email == phone && u.IdAdmin == id);
        }

        public async Task<bool> DoesUserWithUsernameExist(string username, int id)
        {
            return await _context.Users.AnyAsync(u => u.Email == username && u.IdAdmin==id);
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email, int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IdAdmin == id);
        }

        public async Task<User?> DoesUserWithFacebookExist(string id, int admin)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.FacebookId == id && u.IdAdmin == admin);
        }

    }
}
