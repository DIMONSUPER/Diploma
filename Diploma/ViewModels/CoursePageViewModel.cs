using System.Threading.Tasks;
using System.Windows.Input;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Authorization;
using Diploma.Views.Modal;
using Prism.Events;
using Prism.Navigation;

namespace Diploma.ViewModels
{
    public class CoursePageViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public CoursePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IAuthorizationService authorizationService)
            : base(navigationService, eventAggregator)
        {
            _authorizationService = authorizationService;
        }

        #region -- Public properties --

        private CourseBindableModel _currentCourse;
        public CourseBindableModel CurrentCourse
        {
            get => _currentCourse;
            set => SetProperty(ref _currentCourse, value);
        }

        private bool _isAddNewLessonAvailable;
        public bool IsAddNewLessonAvailable
        {
            get => _isAddNewLessonAvailable;
            set => SetProperty(ref _isAddNewLessonAvailable, value);
        }

        private ICommand _addNewLessonButtonTapped;
        public ICommand AddNewLessonButtonTapped => _addNewLessonButtonTapped ??= SingleExecutionCommand.FromFunc(OnAddNewLessonButtonTappedAsync);

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(CourseBindableModel), out CourseBindableModel course))
            {
                CurrentCourse = course;

                IsAddNewLessonAvailable = _authorizationService.UserId == CurrentCourse.TeacherId;
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnBackButtonTappedCommandAsync()
        {
            return NavigationService.GoBackAsync();
        }

        private Task OnAddNewLessonButtonTappedAsync()
        {
            return NavigationService.NavigateAsync(nameof(NewLessonPage), (nameof(CourseBindableModel), CurrentCourse));
        }

        #endregion
    }
}
