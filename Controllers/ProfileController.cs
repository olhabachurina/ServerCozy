using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCozy.Data;
using ServerCozy.DTOs;
using System.Security.Claims;

namespace ServerCozy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Profile
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ProfileDTO>> GetProfile()
        {
            // Получаем идентификатор пользователя из JWT-токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Пользователь не авторизован.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            // Преобразуем сущность User в DTO
            var profileDTO = new ProfileDTO
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender
            };

            return Ok(profileDTO);
        }

        // PUT: api/Profile
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ProfileUpdateDTO profileUpdateDTO)
        {
            // Получаем идентификатор пользователя из JWT-токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Пользователь не авторизован.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            // Проверяем уникальность email, если это необходимо
            if (_context.Users.Any(u => u.Email == profileUpdateDTO.Email && u.Id != user.Id))
            {
                return BadRequest("Email уже используется другим пользователем.");
            }

            // Обновляем данные пользователя из DTO
            user.FirstName = profileUpdateDTO.FirstName;
            user.LastName = profileUpdateDTO.LastName;
            user.Email = profileUpdateDTO.Email;
            user.Gender = profileUpdateDTO.Gender;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Данные профиля успешно обновлены."); // Возвращаем успешный ответ
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}