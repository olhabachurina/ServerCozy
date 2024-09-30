using ServerCozy.Models;

namespace ServerCozy.Interfaces
{
    public interface IProfileService
    {
        User GetProfile(int userId); // Метод для получения профиля пользователя
        void UpdateProfile(User user); // Метод для обновления профиля пользователя
    }
}
