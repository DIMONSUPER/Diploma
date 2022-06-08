using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Enums;
using Diploma.Helpers;
using Diploma.Resources.Strings;
using Diploma.Services.Authorization;
using Diploma.Services.User;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;

namespace Diploma.ViewModels.Modal
{
    public class SignUpPageViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;

        public SignUpPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IAuthorizationService authorizationService,
            IUserDialogs userDialogs,
            IUserService userService)
            : base(navigationService, eventAggregator)
        {
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;
            _userService = userService;

            CurrentState = LayoutState.Success;
        }

        private string TrimmedEmail => Email.Trim().ToLower();

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

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private ObservableCollection<string> _roles;
        public ObservableCollection<string> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        private string _emailWarningText;
        public string EmailWarningText
        {
            get => _emailWarningText;
            set => SetProperty(ref _emailWarningText, value);
        }

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        private ICommand _signUpButtonTappedCommand;
        public ICommand SignUpButtonTappedCommand => _signUpButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSignUpButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(Email))
            {
                if (IsEmailCorrect(TrimmedEmail) && IsInternetConnected)
                {
                    if (await IsEmailDoesntExist(TrimmedEmail))
                    {
                        EmailWarningText = default;
                    }
                    else
                    {
                        EmailWarningText = Strings.UserWithSuchEmail;
                    }
                }
                else
                {
                    EmailWarningText = Strings.EmailDoesntExist;
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task<bool> IsEmailDoesntExist(string email)
        {
            bool result = false;

            var usersResponse = await _userService.GetAllUsersAsync();

            if (usersResponse.IsSuccess)
            {
                result = !usersResponse.Result.Any(x => x.Email == email);
            }

            return result;
        }

        private async Task OnSignUpButtonTappedCommandAsync()
        {
            if (IsInternetConnected)
            {

            }
            else
            {
                await _userDialogs.AlertAsync(Strings.NoInternetConnection);
            }
        }

        private Task OnBackButtonTappedCommandAsync()
        {
            return NavigationService.GoBackAsync(new NavigationParameters(), true, true);
        }

        private bool IsEmailCorrect(string email)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(email))
            {

            }

            return result;
        }

        #endregion
    }
}
