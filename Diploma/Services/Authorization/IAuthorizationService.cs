using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;

namespace Diploma.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<AOResult<UserModel>> LoginAsync(string identifier, string password);

        Task<AOResult<UserModel>> RegisterAsync(UserModel userModel, string password);

        bool IsAuthorized { get; }

        int UserId { get; }
    }
}
