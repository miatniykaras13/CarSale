using BusinessLogic.DTO;
using Data.Filters;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Contracts
{
    public interface IUserService
    {
        public Task<User> GetByEmailAsync(string email);
        public Task<User> GetByIdAsync(Guid id);
        public Task<List<UserDto>> GetAllAsync(UserFilter userFilter);   
        public Task Register(string firstName, string lastName, string email, string password);
        public Task<string> Login(string email, string password);
        public Task DeleteAsync(Guid id);

    }
}
