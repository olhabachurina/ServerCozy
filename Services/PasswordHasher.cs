using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ServerCozy.Services
{
    public class PasswordHasher
    {
        // Генерация соли
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8]; // 128-битная соль (16 байт)
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Хеширование пароля с солью
        public static string HashPassword(string password, byte[] salt)
        {
            // Хеширование пароля с использованием алгоритма PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)); // 256-битный хеш

            return hashed;
        }

        // Проверка пароля (сравнение введенного пароля с хешированным паролем в базе)
        public static bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
        {
            // Хешируем введенный пользователем пароль с сохраненной солью
            string hashedPassword = HashPassword(enteredPassword, storedSalt);
            // Сравниваем хеши
            return hashedPassword == storedHash;
        }
    }
}