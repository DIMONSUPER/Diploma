using Xamarin.Essentials;

namespace Diploma.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public SettingsManager()
        {
            UserSettings = new();
        }

        #region -- ISettingsManager Implementation --

        public UserSettings UserSettings { get; }

        public bool IsAuthCompleted
        {
            get => Preferences.Get(nameof(IsAuthCompleted), false);
            set => Preferences.Set(nameof(IsAuthCompleted), value);
        }

        #endregion
    }
}
