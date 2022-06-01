using Xamarin.Essentials;

namespace Diploma.Services.Settings
{
    public class UserSettings
    {
        #region -- Public properties --

        public int AppTheme
        {
            get => Preferences.Get(nameof(AppTheme), 1);
            set => Preferences.Set(nameof(AppTheme), value);
        }

        public bool ShouldNotifyMe
        {
            get => Preferences.Get(nameof(ShouldNotifyMe), false);
            set => Preferences.Set(nameof(ShouldNotifyMe), value);
        }

        public string CoursesLanguage
        {
            get => Preferences.Get(nameof(CoursesLanguage), Constants.LanguageConstansts.English);
            set => Preferences.Set(nameof(CoursesLanguage), value);
        }

        #endregion
    }
}
