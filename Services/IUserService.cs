using System;
using System.Collections.Generic;
using InsecureEncryptionDemo.Models;

namespace InsecureEncryptionDemo.Services
{
    public interface IUserService
    {
        User CreateUser(string email, string password, string passwordHint);
        User GetUserById(Guid id);
        IEnumerable<User> GetAllUsers();
    }
}