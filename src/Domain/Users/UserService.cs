using GasWeb.Domain.Exceptions;
using GasWeb.Shared.Authentication;
using GasWeb.Shared.Users;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Users
{
    public interface IUserService
    {
        Task<User> Add(RegisterModel registerModel);
        Task<User> TryLogIn(LoginModel logInModel);
        Task<User> Get(long id);
        Task Update(long id, UserUpdateModel updateModel);
        Task<IReadOnlyCollection<User>> GetList();
    }

    internal class UserService : IUserService
    {
        private readonly GasWebDbContext dbContext;

        public UserService(GasWebDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> Add(RegisterModel registerModel)
        {
            var user = new Entities.User(
                registerModel.Username, 
                registerModel.Password,
                registerModel.Role,
                active: true);

            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            return user.ToContract();
        }

        public async Task<User> TryLogIn(LoginModel logInModel)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Name == logInModel.Username);

            if (user == null || user.Password != logInModel.Password)
                return null;

            return user.ToContract();
        }

        public async Task<User> Get(long id)
        {
            var user = await dbContext.Users.GetAsync(id);
            return user?.ToContract();
        }

        public async Task<IReadOnlyCollection<User>> GetList()
        {
            return await dbContext.Users.Select(x => x.ToContract()).ToListAsync();
        }

        public async Task Update(long id, UserUpdateModel updateModel)
        {
            var user = await dbContext.Users.GetAsync(id);
            user.Update(updateModel);
            await dbContext.SaveChangesAsync();
        }
    }
}
