namespace Diploma.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public SettingsManager()
        {
            UserSettings = new();
            AuthorizationSettings = new();
        }

        #region -- ISettingsManager Implementation --

        public UserSettings UserSettings { get; }

        public AuthorizationSettings AuthorizationSettings { get; }

        public bool IsAuthorized { get => AuthorizationSettings.UserId != 0; }

        #endregion
    }
}
