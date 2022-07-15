using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Resources.Strings;
using Diploma.Services.Course;
using Diploma.Services.Settings;
using Diploma.Services.User;
using Prism.Events;
using Prism.Navigation;

namespace Diploma.ViewModels.Modal
{
    public class NewCoursePageViewModel : BaseViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;
        private readonly ICoursesService _coursesService;

        public NewCoursePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ISettingsManager settingsManager,
            IUserDialogs userDialogs,
            IUserService userService,
            ICoursesService coursesService)
            : base(navigationService, eventAggregator)
        {
            _settingsManager = settingsManager;
            _userDialogs = userDialogs;
            _userService = userService;
            _coursesService = coursesService;
        }

        #region -- Public properties --

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _category;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private bool _isAddButtonEnabled;
        public bool IsAddButtonEnabled
        {
            get => _isAddButtonEnabled;
            set => SetProperty(ref _isAddButtonEnabled, value);
        }

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        private ICommand _addButtonTappedCommand;
        public ICommand AddButtonTappedCommand => _addButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnAddButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(Category) or nameof(Title) or nameof(ImageUrl) or nameof(Description))
            {
                IsAddButtonEnabled = !string.IsNullOrWhiteSpace(Category)
                    && !string.IsNullOrWhiteSpace(Category)
                    && !string.IsNullOrWhiteSpace(Title)
                    && !string.IsNullOrWhiteSpace(ImageUrl)
                    && !string.IsNullOrWhiteSpace(Description);
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnBackButtonTappedCommandAsync()
        {
            return NavigationService.GoBackAsync(new NavigationParameters(), true, true);
        }

        private async Task OnAddButtonTappedCommandAsync()
        {
            if (IsInternetConnected)
            {
                var newCourse = new CourseModel
                {
                    TeacherId = _settingsManager.AuthorizationSettings.UserId,
                    Category = Category,
                    Description = Description,
                    ImageUrl = ImageUrl,
                    IsVisible = IsVisible,
                    Language = _settingsManager.UserSettings.CoursesLanguage,
                    Name = Title,
                    UsersIds = new List<int>() { _settingsManager.AuthorizationSettings.UserId },
                };

                var createNewCourseResponse = await _coursesService.PostNewCourseAsync(newCourse);

                if (createNewCourseResponse.IsSuccess)
                {
                    await OnBackButtonTappedCommandAsync();
                }
                else
                {
                    await OnBackButtonTappedCommandAsync();
                }
            }
            else
            {
                await _userDialogs.AlertAsync(Strings.NoInternetConnection);
            }
        }

        #endregion
    }
}
