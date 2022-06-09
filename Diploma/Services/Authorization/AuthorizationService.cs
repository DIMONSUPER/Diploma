using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Repository;
using Diploma.Services.Rest;
using Diploma.Services.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IRestService _restService;
        private readonly IRepositoryService _repositoryService;

        public AuthorizationService(
            ISettingsManager settingsManager,
            IRestService restService,
            IRepositoryService repositoryService)
        {
            _settingsManager = settingsManager;
            _restService = restService;
            _repositoryService = repositoryService;
        }

        #region -- IAuthorizationService implementation --

        public bool IsAuthorized => _settingsManager.IsAuthorized;

        public Task<AOResult<UserModel>> LoginAsync(string identifier, string password)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                UserModel result = null;

                var url = $"{Constants.BASE_URL}/auth/local";

                var response = await _restService.PostAsync<JToken>(url, new Dictionary<string, string>
                {
                    { nameof(identifier), identifier },
                    { nameof(password), password },
                });

                var jwtToken = response.Value<string>("jwt");
                var userJson = response.Value<JContainer>("user");

                if (!string.IsNullOrWhiteSpace(jwtToken) && userJson is not null)
                {
                    result = JsonSerializer.Create().Deserialize<UserModel>(userJson.CreateReader());

                    _settingsManager.AuthorizationSettings.AuthorizeUser(jwtToken, result);

                    await _repositoryService.SaveOrUpdateAsync(result);
                }
                else
                {
                    onFailure("Failed");
                }

                return result;
            });
        }

        public Task<AOResult<UserModel>> RegisterAsync(UserModel userModel, string password)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                UserModel result = null;

                var url = $"{Constants.BASE_URL}/auth/local/register";

                var response = await _restService.PostAsync<JToken>(url, new Dictionary<string, object>
                {
                    { "email", userModel.Email },
                    { "username", userModel.Username },
                    { "password", password },
                    { "role_id", userModel.RoleId },
                    { "name", userModel.Name },
                    { "surname", userModel.Surname },
                    { "description", userModel.Description },
                });

                var jwtToken = response.Value<string>("jwt");
                var userJson = response.Value<JContainer>("user");

                if (!string.IsNullOrWhiteSpace(jwtToken) && userJson is not null)
                {
                    result = JsonSerializer.Create().Deserialize<UserModel>(userJson.CreateReader());

                    _settingsManager.AuthorizationSettings.AuthorizeUser(jwtToken, result);

                    await _repositoryService.SaveOrUpdateAsync(result);
                }
                else
                {
                    onFailure("Failed");
                }

                return result;
            });
        }

        #endregion
    }
}
