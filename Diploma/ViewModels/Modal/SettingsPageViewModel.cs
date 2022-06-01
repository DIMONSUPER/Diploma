using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Diploma.Helpers;
using Diploma.Services.Settings;
using Diploma.Services.Style;
using Prism.Navigation;

namespace Diploma.ViewModels.Modal
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IStyleService _styleService;

        public SettingsPageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager,
            IStyleService styleService)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _styleService = styleService;
        }

        #region -- Public properties --

        private bool _isDarkThemeEnabled;
        public bool IsDarkThemeEnabled
        {
            get => _isDarkThemeEnabled;
            set => SetProperty(ref _isDarkThemeEnabled, value);
        }

        private ObservableCollection<string> _languageItems;
        public ObservableCollection<string> LanguageItems
        {
            get => _languageItems;
            set => SetProperty(ref _languageItems, value);
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            InitLanguageItems();

            IsDarkThemeEnabled = _settingsManager.UserSettings.AppTheme == 2;

            SelectedLanguage = _settingsManager.UserSettings.CoursesLanguage;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsDarkThemeEnabled))
            {
                _settingsManager.UserSettings.AppTheme = IsDarkThemeEnabled ? 2 : 1;
                _styleService.ChangeThemeTo(IsDarkThemeEnabled ? Xamarin.Forms.OSAppTheme.Dark : Xamarin.Forms.OSAppTheme.Light);
            }
            else if (args.PropertyName == nameof(SelectedLanguage) && SelectedLanguage is not null)
            {
                _settingsManager.UserSettings.CoursesLanguage = SelectedLanguage;
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnBackButtonTappedCommandAsync()
        {
            return NavigationService.GoBackAsync(new NavigationParameters(), true, true);
        }

        private void InitLanguageItems()
        {
            LanguageItems = new()
            {
                Constants.LanguageConstansts.English,
                Constants.LanguageConstansts.Russian,
                Constants.LanguageConstansts.Ukrainian,
            };
        }

        #endregion
    }
}
