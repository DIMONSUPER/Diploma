namespace Diploma.Services.Settings
{
    public interface ISettingsManager
    {
        bool IsAuthorized { get; }

        UserSettings UserSettings { get; }

        AuthorizationSettings AuthorizationSettings { get; }
    }
}
