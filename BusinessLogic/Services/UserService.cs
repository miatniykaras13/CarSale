﻿using BusinessLogic.JWT;
using BusinessLogic.Services.Contracts;
using Data.Hashers.Contracts;
using Data.Models;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {

        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;


        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task Register(string firstName, string lastName, string email, string password)
        {
            var passwordHashed = _passwordHasher.Generate(password);

            var user = new User() {Id = Guid.NewGuid(), FirstName = firstName, LastName = lastName, Email = email, Password = passwordHashed};

            await _userRepository.AddAsync(user);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            var result = _passwordHasher.Verify(password, user.Password);

            if (result == false)
                throw new Exception("Invalid password.");

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

       
    }
}
