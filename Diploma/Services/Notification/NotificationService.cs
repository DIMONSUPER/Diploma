using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Rest;
using Diploma.Services.Settings;
using Newtonsoft.Json.Linq;

namespace Diploma.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IRestService _restService;
        private readonly ISettingsManager _settingsManager;

        public NotificationService(
            IRestService restService,
            ISettingsManager settingsManager)
        {
            _restService = restService;
            _settingsManager = settingsManager;
        }

        #region -- INotificationService implementation

        public Task<AOResult<IEnumerable<NotificationModel>>> GetAllNotifications()
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/notifications";

                var response = await _restService.GetAsync<JToken>(url);

                if (response is null)
                {
                    onFailure("response is null");
                }

                return JTokenHelper.ParseFromJToken<NotificationModel>(response).Where(x => x.Language == _settingsManager.UserSettings.CoursesLanguage);
            });
        }

        #endregion
    }
}
