namespace Diploma.Services.Settings
{
    public interface ISettingsManager
    {
        bool IsAuthCompleted { get; set; }

        UserSettings UserSettings { get; }
    }
}
