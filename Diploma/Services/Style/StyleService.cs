using Diploma.Resources.Styles;
using Diploma.Services.Settings;
using Xamarin.Forms;

namespace Diploma.Services.Style
{
    public class StyleService : IStyleService
    {
        private readonly ISettingsManager _settingsManager;

        public StyleService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IStyleService implementation --

        public void SetThemeOnStart()
        {
            if (_settingsManager.UserSettings.AppTheme == 0)
            {
                ChangeThemeTo(App.Current.RequestedTheme);
            }
        }

        public void ChangeThemeTo(OSAppTheme theme)
        {
            _settingsManager.UserSettings.AppTheme = (int)theme;
            App.Current.UserAppTheme = theme;

            App.Current.Resources.MergedDictionaries.Clear();
            App.Current.Resources.MergedDictionaries.Add(
                theme == OSAppTheme.Dark
                ? new DarkModeResources()
                : new LightModeResources());
        }

        #endregion

    }
}
