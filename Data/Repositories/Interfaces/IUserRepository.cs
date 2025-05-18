using Data.Filters;
using Data.Models;
using Data.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(User c);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(User c);
        public Task<User> GetByIdAsync(Guid id);
        public Task<User> GetByEmailAsync(string email);
        public Task<List<User>> GetAllAsync(UserFilter userFilter, SortParameters sortParameters);
    }
}
