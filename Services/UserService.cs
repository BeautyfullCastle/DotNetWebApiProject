using DotNetWebApiProject.Models;
using DotNetWebApiProject.Repositories;
using DotNetWebApiProject.Cache;

namespace DotNetWebApiProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;

        public UserService(IUserRepository userRepository, ICacheService cacheService)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var cacheKey = $"User:{id}";
            var user = await _cacheService.GetAsync<User>(cacheKey);
            if (user != null)
            {
                return user;
            }

            user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                await _cacheService.SetAsync(cacheKey, user, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var cacheKey = "AllUsers";
            var users = await _cacheService.GetAsync<IEnumerable<User>>(cacheKey);
            if (users != null)
            {
                return users;
            }

            users = await _userRepository.GetAllUsersAsync();
            if (users != null)
            {
                await _cacheService.SetAsync(cacheKey, users, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            }
            return users;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.AddUserAsync(user);
            await _cacheService.SetAsync($"User:{user.Id}", user, TimeSpan.FromMinutes(5));
            return user;
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                // Or throw an exception
                return;
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            await _userRepository.UpdateUserAsync(existingUser);
            await _cacheService.SetAsync($"User:{existingUser.Id}", existingUser, TimeSpan.FromMinutes(5));
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            await _cacheService.RemoveAsync($"User:{id}");
        }
    }
}
