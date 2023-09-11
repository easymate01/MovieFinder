using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task AddUser(User user);
        Task UpdateUser(User user);
    }
}
