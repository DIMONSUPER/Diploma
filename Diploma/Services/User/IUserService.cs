using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;

namespace Diploma.Services.User
{
    public interface IUserService
    {
        Task<AOResult<IEnumerable<UserModel>>> GetAllUsersAsync();

        Task<AOResult<UserModel>> UpdateUserAsync(UserModel userModel);

        Task<AOResult<UserModel>> GetUserByIdAsync(int id);
    }
}
