using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Resources.Strings;
using Diploma.Services.User;
using Prism.Events;
using Prism.Navigation;

namespace Diploma.ViewModels.Modal
{
    public class EditProfilePageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;
        private UserModel _currentUser;

        public EditProfilePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IUserDialogs userDialogs,
            IUserService userService)
            : base(navigationService, eventAggregator)
        {
            _userDialogs = userDialogs;
            _userService = userService;
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

        private bool _isSaveButtonEnabled;
        public bool IsSaveButtonEnabled
        {
            get => _isSaveButtonEnabled;
            set => SetProperty(ref _isSaveButtonEnabled, value);
        }

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        private ICommand _saveButtonTappedCommand;
        public ICommand SaveButtonTappedCommand => _saveButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnSaveButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(UserModel), out UserModel userModel))
            {
                FirstName = userModel.Name;
                LastName = userModel.Surname;
                Description = userModel.Description;
                _currentUser = userModel;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(FirstName) or nameof(LastName))
            {
                IsSaveButtonEnabled = !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnSaveButtonTappedCommandAsync()
        {
            if (IsInternetConnected)
            {
                _currentUser.Description = Description;
                _currentUser.Name = FirstName;
                _currentUser.Surname = LastName;

                _userDialogs.ShowLoading(Strings.Loading);

                var editResponse = await _userService.UpdateUserAsync(_currentUser);

                _userDialogs.HideLoading();

                if (editResponse.IsSuccess)
                {
                    await NavigationService.GoBackAsync(new NavigationParameters { { nameof(UserModel), _currentUser } }, true, true);
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

        #endregion
    }
}
