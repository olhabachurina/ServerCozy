namespace ServerCozy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }  // Имя пользователя

        public string PasswordHash { get; set; }  // Хеш пароля
        public string PasswordSalt { get; set; }  // Соль для пароля

        public string FirstName { get; set; }  // Имя
        public string LastName { get; set; }   // Фамилия
        public string Gender { get; set; }     // Пол
        public string Email { get; set; }      // Электронная почта

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Дата создания пользователя
    }
}
  