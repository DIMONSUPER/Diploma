using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Resources.Strings;
using Diploma.Services.Authorization;
using Diploma.Services.User;
using Prism.Events;
using Prism.Navigation;

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
        }

        private string TrimmedEmail => Email?.Trim().ToLower();
        private string TrimmedUsername => Username?.Trim().ToLower();

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

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
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

        private string _usernameWarningText;
        public string UsernameWarningText
        {
            get => _usernameWarningText;
            set => SetProperty(ref _usernameWarningText, value);
        }

        private string _passwordWarningText;
        public string PasswordWarningText
        {
            get => _passwordWarningText;
            set => SetProperty(ref _passwordWarningText, value);
        }

        private string _confirmPasswordWarningText;
        public string ConfirmPasswordWarningText
        {
            get => _confirmPasswordWarningText;
            set => SetProperty(ref _confirmPasswordWarningText, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private bool _isSignUpButtonEnabled = true;
        public bool IsSignUpButtonEnabled
        {
            get => _isSignUpButtonEnabled;
            set => SetProperty(ref _isSignUpButtonEnabled, value);
        }

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        private ICommand _signUpButtonTappedCommand;
        public ICommand SignUpButtonTappedCommand => _signUpButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSignUpButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            InitRoles();

            SelectedRole = Roles.FirstOrDefault();
        }

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
            else if (args.PropertyName is nameof(Username))
            {
                if (!string.IsNullOrWhiteSpace(TrimmedUsername) && TrimmedUsername.Length >= 4 && IsInternetConnected)
                {
                    if (await IsUsernameDoesntExist(TrimmedUsername))
                    {
                        UsernameWarningText = default;
                    }
                    else
                    {
                        UsernameWarningText = Strings.UserWithSuchUsername;
                    }
                }
                else
                {
                    UsernameWarningText = Strings.UserMustContain;
                }
            }
            else if (args.PropertyName is nameof(Password) or nameof(ConfirmPassword))
            {
                if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                {
                    PasswordWarningText = Strings.PasswordMustContain;
                }
                else
                {
                    PasswordWarningText = default;
                }

                if (string.IsNullOrWhiteSpace(ConfirmPassword) || ConfirmPassword != Password)
                {
                    ConfirmPasswordWarningText = Strings.PasswordsDontMatch;
                }
                else
                {
                    ConfirmPasswordWarningText = default;
                }
            }

            if (args.PropertyName is nameof(Password) or nameof(Username) or nameof(ConfirmPassword) or nameof(Email) or nameof(FirstName) or nameof(LastName))
            {
                IsSignUpButtonEnabled = !string.IsNullOrWhiteSpace(Password)
                    && string.IsNullOrWhiteSpace(PasswordWarningText)
                    && string.IsNullOrWhiteSpace(ConfirmPasswordWarningText)
                    && !string.IsNullOrWhiteSpace(FirstName)
                    && !string.IsNullOrWhiteSpace(LastName)
                    && !string.IsNullOrWhiteSpace(TrimmedUsername)
                    && !string.IsNullOrWhiteSpace(TrimmedEmail)
                    && string.IsNullOrWhiteSpace(EmailWarningText)
                    && string.IsNullOrWhiteSpace(UsernameWarningText);
            }
        }

        #endregion

        #region -- Private helpers --

        private void InitRoles()
        {
            Roles = new()
            {
                Strings.Student,
                Strings.Teacher,
            };

        }

        private async Task<bool> IsUsernameDoesntExist(string username)
        {
            bool result = false;

            var usersResponse = await _userService.GetAllUsersAsync();

            if (usersResponse.IsSuccess)
            {
                result = !usersResponse.Result.Any(x => x.Username.ToLower() == username);
            }

            return result;
        }

        private async Task<bool> IsEmailDoesntExist(string email)
        {
            bool result = false;

            var usersResponse = await _userService.GetAllUsersAsync();

            if (usersResponse.IsSuccess)
            {
                result = !usersResponse.Result.Any(x => x.Email.ToLower() == email);
            }

            return result;
        }

        private async Task OnSignUpButtonTappedCommandAsync()
        {
            if (IsInternetConnected)
            {
                var userModel = new UserModel
                {
                    Description = Description,
                    Email = TrimmedEmail,
                    Name = FirstName,
                    Surname = LastName,
                    Username = TrimmedUsername,
                    RoleId = SelectedRole == Strings.Student ? 0 : 1,
                };

                _userDialogs.ShowLoading(Strings.Loading);

                var registrationResponse = await _authorizationService.RegisterAsync(userModel, Password);

                _userDialogs.HideLoading();

                if (registrationResponse.IsSuccess)
                {
                    await OnBackButtonTappedCommandAsync();
                }
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
            if (email.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
