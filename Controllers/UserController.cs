using System;
using Microsoft.AspNetCore.Mvc;
using InsecureEncryptionDemo.Models;
using InsecureEncryptionDemo.Services;

namespace InsecureEncryptionDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;

        public UserController(IUserService userService, IEncryptionService encryptionService)
        {
            _userService = userService;
            _encryptionService = encryptionService;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.PasswordHint))
            {
                return BadRequest("Email, password, and password hint are required.");
            }

            var user = _userService.CreateUser(request.Email, request.Password, request.PasswordHint);
            return Ok(new { user.Id, user.Email, Message = "User created successfully" });
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.EncryptedPassword,
                PasswordHint = _encryptionService.IsSecureMode ? _encryptionService.Decrypt(user.PasswordHint) : user.PasswordHint
            });
        }
    }

    public class SignupRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHint { get; set; }
    }
}