using System;
using System.Collections.Generic;
using System.Linq;
using InsecureEncryptionDemo.Models;

namespace InsecureEncryptionDemo.Services
{
    public class UserService : IUserService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly List<User> _users = new List<User>();

        public UserService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public User CreateUser(string email, string password, string passwordHint)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                EncryptedPassword = _encryptionService.Encrypt(password),
                PasswordHint = _encryptionService.IsSecureMode 
                    ? _encryptionService.Encrypt(passwordHint)
                    : passwordHint
            };

            _users.Add(user);
            return user;
        }

        public User GetUserById(Guid id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }
    }
}