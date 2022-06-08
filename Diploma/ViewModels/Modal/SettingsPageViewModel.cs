using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Diploma.Events;
using Diploma.Helpers;
using Diploma.Resources.Strings;
using Diploma.Services.Settings;
using Diploma.Services.Style;
using Plugin.LocalNotification;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.Helpers;

namespace Diploma.ViewModels.Modal
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IStyleService _styleService;
        private readonly INotificationService _notificationService;
        private bool _isInitialized;

        public SettingsPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ISettingsManager settingsManager,
            IStyleService styleService,
            INotificationService notificationService)
            : base(navigationService, eventAggregator)
        {
            _settingsManager = settingsManager;
            _styleService = styleService;
            _notificationService = notificationService;
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

        private bool _shouldNotifyMe;
        public bool ShouldNotifyMe
        {
            get => _shouldNotifyMe;
            set => SetProperty(ref _shouldNotifyMe, value);
        }

        private TimeSpan _notificationTime;
        public TimeSpan NotificationTime
        {
            get => _notificationTime;
            set => SetProperty(ref _notificationTime, value);
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

            ShouldNotifyMe = _settingsManager.UserSettings.ShouldNotifyMe;
            NotificationTime = _settingsManager.UserSettings.NotificationTime;

            _isInitialized = true;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(IsDarkThemeEnabled))
            {
                _settingsManager.UserSettings.AppTheme = IsDarkThemeEnabled ? 2 : 1;
                _styleService.ChangeThemeTo(IsDarkThemeEnabled ? Xamarin.Forms.OSAppTheme.Dark : Xamarin.Forms.OSAppTheme.Light);
            }
            else if (args.PropertyName is nameof(SelectedLanguage) && SelectedLanguage is not null)
            {
                LocalizationResourceManager.Current.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = GetCultureInfoFromLanguage(SelectedLanguage);
                _settingsManager.UserSettings.CoursesLanguage = SelectedLanguage;
                EventAggregator.GetEvent<LanguageChangedEvent>().Publish(SelectedLanguage);
            }
            else if (_isInitialized && args.PropertyName is nameof(ShouldNotifyMe) or nameof(NotificationTime))
            {
                SetNotification();
            }
        }

        #endregion

        #region -- Private helpers --

        private void SetNotification()
        {
            _notificationService.CancelAll();

            if (ShouldNotifyMe)
            {
                _notificationService.Show(new NotificationRequest()
                {
                    BadgeNumber = 1,
                    Description = Strings.ContinueYourStudying,
                    Title = Strings.DailyRemainder,
                    NotificationId = 16,
                    Schedule = new()
                    {
                        NotifyTime = DateTime.Today.Add(NotificationTime),
                        RepeatType = NotificationRepeat.Daily,
                    }
                });
            }

            _settingsManager.UserSettings.ShouldNotifyMe = ShouldNotifyMe;
            _settingsManager.UserSettings.NotificationTime = NotificationTime;
        }

        private CultureInfo GetCultureInfoFromLanguage(string language)
        {
            return language switch
            {
                Constants.LanguageConstansts.English => new("en-US"),
                Constants.LanguageConstansts.Russian => new("ru-RU"),
                Constants.LanguageConstansts.Ukrainian => new("uk-UA"),
                _ => throw new System.NotImplementedException(),
            };
        }

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
