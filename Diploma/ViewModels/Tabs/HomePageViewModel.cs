using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Diploma.Events;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Authorization;
using Diploma.Services.Course;
using Diploma.Services.Mapper;
using Diploma.Services.User;
using Diploma.Views;
using Diploma.Views.Modal;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace Diploma.ViewModels.Tabs
{
    public class HomePageViewModel : BaseTabViewModel
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICoursesService _coursesService;
        private readonly IMapperService _mapperService;

        public HomePageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IUserService userService,
            IAuthorizationService authorizationService,
            ICoursesService coursesService,
            IMapperService mapperService)
            : base(navigationService, eventAggregator)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _coursesService = coursesService;
            _mapperService = mapperService;

            EventAggregator.GetEvent<LanguageChangedEvent>().Subscribe(OnLanguageChanged);

            CurrentState = LayoutState.Loading;
        }

        #region -- Public properties --

        private ObservableCollection<CarouselBindableModel> _homeItems = new();
        public ObservableCollection<CarouselBindableModel> HomeItems
        {
            get => _homeItems;
            set => SetProperty(ref _homeItems, value);
        }

        private bool _isAddNewCourseAvailable;
        public bool IsAddNewCourseAvailable
        {
            get => _isAddNewCourseAvailable;
            set => SetProperty(ref _isAddNewCourseAvailable, value);
        }

        private ICommand _addNewCourseButtonTapped;
        public ICommand AddNewCourseButtonTapped => _addNewCourseButtonTapped ??= SingleExecutionCommand.FromFunc(OnAddNewCourseButtonTappedAsync);

        private ICommand _courseTappedCommand;
        public ICommand CourseTappedCommand => _courseTappedCommand ??= SingleExecutionCommand.FromFunc<CourseBindableModel>(OnCourseTappedCommandAsync);

        private ICommand _lessonTappedCommand;
        public ICommand LessonTappedCommand => _lessonTappedCommand ??= SingleExecutionCommand.FromFunc<LessonBindableModel>(OnLessonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            CurrentState = LayoutState.Loading;

            await UpdateCoursesAsync();

            if (_authorizationService.IsAuthorized)
            {
                var userResponse = await _userService.GetUserByIdAsync(_authorizationService.UserId);

                if (userResponse.IsSuccess)
                {
                    IsAddNewCourseAvailable = userResponse.Result.RoleId == 1;
                }
            }
        }

        protected override async void OnConnectionChanged(object sender, ConnectivityChangedEventArgs e)
        {
            base.OnConnectionChanged(sender, e);

            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                await UpdateCoursesAsync();
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnLessonTappedCommandAsync(LessonBindableModel lesson)
        {
            return NavigationService.NavigateAsync(nameof(LessonPage), (nameof(LessonBindableModel), lesson));
        }

        private Task OnCourseTappedCommandAsync(CourseBindableModel course)
        {
            return NavigationService.NavigateAsync(nameof(CoursePage), (nameof(CourseBindableModel), course));
        }

        private Task OnAddNewCourseButtonTappedAsync()
        {
            return NavigationService.NavigateAsync(nameof(NewCoursePage), new NavigationParameters(), true, true);
        }

        private async Task UpdateCoursesAsync()
        {
            CurrentState = LayoutState.Loading;

            var coursesResponse = await _coursesService.GetAllCoursesAsync();

            var tmpCollection = new List<CarouselBindableModel>();

            if (coursesResponse.IsSuccess)
            {
                var courses = (await _coursesService.ConvertToBindableCourses(coursesResponse.Result)).ToList();

                foreach (var course in courses)
                {
                    course.TappedCommand = CourseTappedCommand;

                    foreach (var lesson in course.Lessons)
                    {
                        lesson.TappedCommand = LessonTappedCommand;
                    }
                }

                foreach (var group in courses.GroupBy(x => x.Category).OrderBy(x => x.Key))
                {
                    tmpCollection.Add(new() { Items = new(group), Title = group.Key });
                }
            }

            HomeItems = new(tmpCollection);

            if (HomeItems.Any())
            {
                CurrentState = LayoutState.Success;
            }
            else if (IsInternetConnected)
            {
                CurrentState = LayoutState.Empty;
            }
            else
            {
                CurrentState = LayoutState.Error;
            }
        }

        private async void OnLanguageChanged(string language)
        {
            HomeItems = new();

            await UpdateCoursesAsync();
        }

        #endregion
    }
}
