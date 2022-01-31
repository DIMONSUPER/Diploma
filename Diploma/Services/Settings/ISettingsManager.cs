namespace Diploma.Services.Settings
{
    public interface ISettingsManager
    {
        bool IsAuthCompleted { get; set; }

        int AppTheme { get; set; }
    }
}
