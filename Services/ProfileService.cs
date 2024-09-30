using ServerCozy.Data;
using ServerCozy.Interfaces;
using ServerCozy.Models;

namespace ServerCozy.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;

        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для получения профиля пользователя
        public User GetProfile(int userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            return user;
        }

        // Метод для обновления профиля пользователя
        public void UpdateProfile(User updatedUser)
        {
            var existingUser = _context.Users.SingleOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser == null)
            {
                throw new Exception("Пользователь не найден");
            }

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Gender = updatedUser.Gender;
            existingUser.Email = updatedUser.Email;

            _context.SaveChanges();
        }
    }
}
