using Microsoft.EntityFrameworkCore;
using MovieFInderServer.Datas;
using MovieFInderServer.Models;

namespace MovieFInderServer.Services.Users
{
    public class UserService : IUserService
    {
        public async Task<User> GetUserByIdAsync(int userId)
        {
            using var dbContext = new MovieFinderContext();
            return await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task AddUser(User user)
        {
            using var dbContext = new MovieFinderContext();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                using var dbContext = new MovieFinderContext();
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
