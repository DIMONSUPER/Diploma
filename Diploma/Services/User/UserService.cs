using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRestService _restService;

        public UserService(IRestService restService)
        {
            _restService = restService;
        }

        #region -- IUserService implementation --

        public Task<AOResult<IEnumerable<UserModel>>> GetAllUsersAsync()
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/users";

                var response = await _restService.GetAsync<IEnumerable<UserModel>>(url);

                if (response is null)
                {
                    onFailure("No users");
                }

                return response;
            });
        }

        public Task<AOResult<UserModel>> GetUserByIdAsync(int id)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/users?filters[id][$eq]={id}";

                var response = await _restService.GetAsync<IEnumerable<UserModel>>(url);

                var result = response.FirstOrDefault();

                if (result is null)
                {
                    onFailure("User not found");
                }

                return result;
            });
        }

        public Task<AOResult<UserModel>> RegisterNewUserAsync(UserModel userModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<AOResult<UserModel>> UpdateUserAsync(UserModel userModel)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
