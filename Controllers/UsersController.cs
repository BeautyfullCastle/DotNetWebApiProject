using DotNetWebApiProject.DTOs;
using DotNetWebApiProject.Models;
using DotNetWebApiProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetWebApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserViewDto { Id = u.Id, Username = u.Username, Email = u.Email, CreatedAt = u.CreatedAt });
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = new UserViewDto { Id = user.Id, Username = user.Username, Email = user.Email, CreatedAt = user.CreatedAt };
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserViewDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var user = new User { Username = userCreateDto.Username, Email = userCreateDto.Email };
            var newUser = await _userService.CreateUserAsync(user);
            var userDto = new UserViewDto { Id = newUser.Id, Username = newUser.Username, Email = newUser.Email, CreatedAt = newUser.CreatedAt };
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto userCreateDto)
        {
            var user = new User { Username = userCreateDto.Username, Email = userCreateDto.Email };
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
