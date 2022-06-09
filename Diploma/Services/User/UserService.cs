using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Repository;
using Diploma.Services.Rest;

namespace Diploma.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRestService _restService;
        private readonly IRepositoryService _repositoryService;

        public UserService(
            IRestService restService,
            IRepositoryService repositoryService)
        {
            _restService = restService;
            _repositoryService = repositoryService;
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

        public Task<AOResult<UserModel>> UpdateUserAsync(UserModel userModel)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/users/{userModel.Id}";

                var body = new { name = userModel.Name, surname = userModel.Surname };

                var response = await _restService.PutAsync<UserModel>(url, body);

                if (response is null)
                {
                    onFailure("Failed to update");
                }

                await _repositoryService.SaveOrUpdateAsync(response);

                return response;
            });
        }

        #endregion
    }
}
