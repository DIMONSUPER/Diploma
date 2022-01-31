using Xamarin.Essentials;

namespace Diploma.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public SettingsManager()
        {
        }

        #region -- ISettingsManager Implementation --

        public bool IsAuthCompleted
        {
            get => Preferences.Get(nameof(IsAuthCompleted), false);
            set => Preferences.Set(nameof(IsAuthCompleted), value);
        }

        public int AppTheme
        {
            get => Preferences.Get(nameof(AppTheme), 0);
            set => Preferences.Set(nameof(AppTheme), value);
        }

        #endregion
    }
}
