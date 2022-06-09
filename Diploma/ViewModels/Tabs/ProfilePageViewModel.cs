using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Events;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Resources.Strings;
using Diploma.Services.Authorization;
using Diploma.Services.Settings;
using Diploma.Services.User;
using Diploma.Views.Modal;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;

namespace Diploma.ViewModels.Tabs
{
    public class ProfilePageViewModel : BaseTabViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;
        private UserModel _currentUser;

        public ProfilePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ISettingsManager settingsManager,
            IAuthorizationService authorizationService,
            IUserDialogs userDialogs,
            IUserService userService)
            : base(navigationService, eventAggregator)
        {
            _settingsManager = settingsManager;
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;
            _userService = userService;

            EventAggregator.GetEvent<AuthChangedEvent>().Subscribe(OnAuthChangedEvent);

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

        private ICommand _signUpButtonTappedCommand;
        public ICommand SignUpButtonTappedCommand => _signUpButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSignUpButtonTappedCommandAsync);

        private ICommand _signOutButtonTappedCommand;
        public ICommand SignOutButtonTappedCommand => _signOutButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSignOutButtonTappedCommandAsync);

        private ICommand _editButtonTappedCommand;
        public ICommand EditButtonTappedCommand => _editButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnEditButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(UserModel), out UserModel userModel))
            {
                FirstName = userModel.Name;
                LastName = userModel.Surname;
                Description = userModel.Description;
                _currentUser = userModel;
            }
        }

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
                    _currentUser = currentUserResponse.Result;
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
                    Identifier.Trim().Length >= 4;
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            EventAggregator.GetEvent<AuthChangedEvent>().Subscribe(OnAuthChangedEvent);
        }

        #endregion

        #region -- Private helpers --

        private async Task OnEditButtonTappedCommandAsync()
        {
            var parameters = new NavigationParameters();

            parameters.Add(nameof(UserModel), _currentUser);

            await NavigationService.NavigateAsync(nameof(EditProfilePage), parameters, true, true);
        }

        private async void OnAuthChangedEvent(bool isLoggedIn)
        {
            if (isLoggedIn)
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

        private Task OnSignUpButtonTappedCommandAsync()
        {
            return NavigationService.NavigateAsync(nameof(SignUpPage), new NavigationParameters(), true, true);
        }

        private async Task OnSignOutButtonTappedCommandAsync()
        {
            var isConfirmed = await _userDialogs.ConfirmAsync(Strings.LogOutMessage);

            if (isConfirmed)
            {
                CurrentState = LayoutState.Loading;

                _settingsManager.AuthorizationSettings.ResetSettings();
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
                    _userDialogs.Alert(Strings.NoInternetConnection);
                    CurrentState = LayoutState.Empty;
                }
            }
            else
            {
                _userDialogs.Alert(Strings.NoInternetConnection);
                CurrentState = LayoutState.Empty;
            }
        }

        private Task OnSettingsButtonTappedCommandAsync()
        {
            return NavigationService.NavigateAsync(nameof(SettingsPage), new NavigationParameters(), true, true);
        }

        private void SetUserInformation(UserModel user)
        {
            FirstName = user.Name;
            LastName = user.Surname;
            Description = user.Description;

            CurrentState = LayoutState.Success;
        }

        #endregion
    }
}
