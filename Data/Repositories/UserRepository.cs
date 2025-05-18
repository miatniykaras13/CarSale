using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Data.Filters;
using Data.Extensions;
using Data.Sorting;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(User u)
        {
            _logger.LogInformation($"Adding new user...");
            await _context.Users.AddAsync(u);
            _logger.LogInformation($"New user with id {u.Id} was added...");
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation($"Searching for user with id {id}...");
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} was not found");
            }
            _logger.LogInformation($"Deleting user with id {id}...");
            _context.Users.Remove(user);
            _logger.LogInformation($"User with id {id} was deleted");
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User u)
        {
            _logger.LogInformation($"Updating User with id {u.Id}...");
            _context.Users.Update(u);
            _logger.LogInformation($"User with id {u.Id} was updated");
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Searching for user with id {id}...");
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _logger.LogInformation($"User with id {id} was found");
                return user;
            }
            throw new KeyNotFoundException($"User with id {id} was not found");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            _logger.LogInformation($"Searching for user with email {email}...");
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} was not found");
            _logger.LogInformation($"User with email {email} was found");
            return user;
        }

        public async Task<List<User>> GetAllAsync(UserFilter userFilter, SortParameters sortParameters)
        {
            _logger.LogInformation($"Getting all users...");

            return await _context.Users.AsNoTracking().Filter(userFilter).Sort(sortParameters).ToListAsync();
        }
        
    }
}
