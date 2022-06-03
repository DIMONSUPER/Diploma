using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Diploma.Helpers;
using Diploma.Resources.Strings;
using Diploma.Services.Settings;
using Diploma.Views.Modal;
using Plugin.LocalNotification;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;

namespace Diploma.ViewModels.Tabs
{
    public class ProfilePageViewModel : BaseTabViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly INotificationService _notificationService;

        public ProfilePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ISettingsManager settingsManager,
            INotificationService notificationService)
            : base(navigationService, eventAggregator)
        {
            _settingsManager = settingsManager;
            _notificationService = notificationService;

            CurrentState = LayoutState.Loading;
        }

        #region -- Public properties --

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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

        private ICommand _settingsButtonTappedCommand;
        public ICommand SettingsButtonTappedCommand => _settingsButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSettingsButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SetUserInformation();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(ShouldNotifyMe) or nameof(NotificationTime))
            {
                SetNotification();
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnSettingsButtonTappedCommandAsync()
        {
            return NavigationService.NavigateAsync(nameof(SettingsPage), new NavigationParameters(), true, true);
        }

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

        private void SetUserInformation()
        {
            FirstName = "Dima";
            LastName = "Fedchenko";
            Description = "Fourth-year student of Dnypro National University";
            _shouldNotifyMe = _settingsManager.UserSettings.ShouldNotifyMe;
            _notificationTime = _settingsManager.UserSettings.NotificationTime;

            CurrentState = LayoutState.Success;
        }

        #endregion
    }
}
