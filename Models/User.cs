using System;

namespace InsecureEncryptionDemo.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
        public string PasswordHint { get; set; }
    }
}