using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Resources.Strings;
using Diploma.Services.Authorization;
using Diploma.Services.Settings;
using Diploma.Services.User;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;

        public ProfilePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ISettingsManager settingsManager,
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            IUserDialogs userDialogs,
            IUserService userService)
            : base(navigationService, eventAggregator)
        {
            _settingsManager = settingsManager;
            _notificationService = notificationService;
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;
            _userService = userService;

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

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _identifier;
        public string Identifier
        {
            get => _identifier;
            set => SetProperty(ref _identifier, value);
        }

        private bool _isSignInButtonEnabled;
        public bool IsSignInButtonEnabled
        {
            get => _isSignInButtonEnabled;
            set => SetProperty(ref _isSignInButtonEnabled, value);
        }

        private ICommand _settingsButtonTappedCommand;
        public ICommand SettingsButtonTappedCommand => _settingsButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSettingsButtonTappedCommandAsync);

        private ICommand _signInTappedCommand;
        public ICommand SignInTappedCommand => _signInTappedCommand ??= SingleExecutionCommand.FromFunc(OnSignInTappedCommandAsync);

        private ICommand _signOutButtonTappedCommand;
        public ICommand SignOutButtonTappedCommand => _signOutButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSignOutButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (_settingsManager.IsAuthorized)
            {
                CurrentState = LayoutState.Success;

                var currentUserResponse = await _userService.GetUserByIdAsync(_settingsManager.AuthorizationSettings.UserId);

                if (currentUserResponse.IsSuccess)
                {
                    SetUserInformation(currentUserResponse.Result);
                }
            }
            else
            {
                CurrentState = LayoutState.Empty;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(Identifier) or nameof(Password))
            {
                IsSignInButtonEnabled = !string.IsNullOrWhiteSpace(Identifier?.Trim()) &&
                    !string.IsNullOrWhiteSpace(Password) &&
                    Identifier.Trim().Length > 4;
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnSignOutButtonTappedCommandAsync()
        {
            var isConfirmed = await _userDialogs.ConfirmAsync(Strings.LogOutMessage);

            if (isConfirmed)
            {
                _settingsManager.AuthorizationSettings.ResetSettings();

                CurrentState = LayoutState.Empty;
            }
        }

        private async Task OnSignInTappedCommandAsync()
        {
            CurrentState = LayoutState.Loading;

            if (IsInternetConnected)
            {
                var loginResponse = await _authorizationService.LoginAsync(Identifier.Trim(), Password);

                if (loginResponse.IsSuccess)
                {
                    SetUserInformation(loginResponse.Result);
                    CurrentState = LayoutState.Success;
                    Identifier = default;
                    Password = default;
                }
                else if (IsInternetConnected)
                {
                    _userDialogs.Alert(Strings.IncorrectLoginOrPassword);
                    CurrentState = LayoutState.Empty;
                }
                else
                {
                    await _userDialogs.AlertAsync(Strings.NoInternetConnection);
                }
            }
            else
            {
                await _userDialogs.AlertAsync(Strings.NoInternetConnection);
            }
        }

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

        private void SetUserInformation(UserModel user)
        {
            FirstName = user.Name;
            LastName = user.Surname;
            Description = user.Description;
            //_shouldNotifyMe = _settingsManager.UserSettings.ShouldNotifyMe;
            //_notificationTime = _settingsManager.UserSettings.NotificationTime;

            CurrentState = LayoutState.Success;
        }

        #endregion
    }
}
