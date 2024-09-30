using Microsoft.AspNetCore.Mvc;
using ServerCozy.DTOs;
using ServerCozy.Interfaces;
using ServerCozy.Models;
using ServerCozy.Services;

namespace ServerCozy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtTokenService _jwtTokenService;

        // Инъекция зависимостей через конструктор
        public AuthController(IAuthService authService, JwtTokenService jwtTokenService)
        {
            _authService = authService;
            _jwtTokenService = jwtTokenService;  // Инициализируем JwtTokenService
        }

        // Регистрация
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Основная логика регистрации
                if (_authService.UserExists(registerDTO.Username))
                {
                    return BadRequest(new { message = "Пользователь с таким именем уже существует." });
                }

                var salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.HashPassword(registerDTO.Password, salt);

                var user = new User
                {
                    Username = registerDTO.Username,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    Gender = registerDTO.Gender,
                    Email = registerDTO.Email,
                    PasswordHash = hashedPassword,
                    PasswordSalt = Convert.ToBase64String(salt)
                };

                _authService.Register(user, registerDTO.Password);

                // Генерация JWT токена сразу после регистрации
                var token = _jwtTokenService.GenerateToken(user);

                // Возвращаем токен и сообщение в формате JSON
                return Ok(new { message = "Пользователь успешно зарегистрирован.", token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                return StatusCode(500, new { message = "Произошла ошибка на сервере." });
            }
        }
        // Тестовый метод
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API работает!");
        }

        // Вход
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var user = _authService.GetUserByUsername(loginDTO.Username);

            if (user == null || !PasswordHasher.VerifyPassword(loginDTO.Password, user.PasswordHash, Convert.FromBase64String(user.PasswordSalt)))
            {
                return Unauthorized(new { message = "Неверное имя пользователя или пароль." });
            }

            // Генерация JWT токена
            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}