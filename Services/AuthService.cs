using Microsoft.AspNetCore.Identity;
using ServerCozy.Data;
using ServerCozy.Models;
using ServerCozy.Interfaces;
using System.Linq;
using ServerCozy.DTOs;
namespace ServerCozy.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
      
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }
        // Реализация метода регистрации
        public void Register(User user, string password)
        {
            // Генерация соли и хеширование пароля
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(password, salt);

            user.PasswordHash = hashedPassword;
            user.PasswordSalt = Convert.ToBase64String(salt);
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }
        // Реализация метода входа
        public User Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                return null; // Пользователь не найден
            }

            var salt = Convert.FromBase64String(user.PasswordSalt);  // Соль в базе хранится в виде строки Base64
            var isValidPassword = PasswordHasher.VerifyPassword(password, user.PasswordHash, salt);

            if (isValidPassword)
            {
                return user; // Вход успешен
            }

            return null; // Неверный пароль
        }
    }
}
        