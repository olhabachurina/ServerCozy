using ServerCozy.Models;

namespace ServerCozy.Interfaces
{
    public interface IAuthService
    {
        bool UserExists(string username);
        void Register(User user, string password);  // Добавляем сюда второй параметр
        User GetUserByUsername(string username);    // Добавляем метод получения пользователя по имени
    }
}
