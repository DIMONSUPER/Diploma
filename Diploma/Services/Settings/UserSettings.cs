using System;
using Xamarin.Essentials;

namespace Diploma.Services.Settings
{
    public class UserSettings
    {
        #region -- Public properties --

        public int AppTheme
        {
            get => Preferences.Get(nameof(AppTheme), 2);
            set => Preferences.Set(nameof(AppTheme), value);
        }

        public bool ShouldNotifyMe
        {
            get => Preferences.Get(nameof(ShouldNotifyMe), false);
            set => Preferences.Set(nameof(ShouldNotifyMe), value);
        }

        public TimeSpan NotificationTime
        {
            get => new(NotificationHour, NotificationMinute, 0);
            set
            {
                NotificationHour = value.Hours;
                NotificationMinute = value.Minutes;
            }
        }

        public string CoursesLanguage
        {
            get => Preferences.Get(nameof(CoursesLanguage), Constants.LanguageConstansts.English);
            set => Preferences.Set(nameof(CoursesLanguage), value);
        }

        #endregion

        #region -- Private properties --

        private int NotificationHour
        {
            get => Preferences.Get(nameof(NotificationHour), 12);
            set => Preferences.Set(nameof(NotificationHour), value);
        }

        private int NotificationMinute
        {
            get => Preferences.Get(nameof(NotificationMinute), 0);
            set => Preferences.Set(nameof(NotificationMinute), value);
        }

        #endregion
    }
}
