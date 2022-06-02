using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Models;
using Diploma.Services.Authorization;
using Diploma.Services.Course;
using Diploma.Services.Mapper;
using Diploma.Services.User;
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
            IUserService userService,
            IAuthorizationService authorizationService,
            ICoursesService coursesService,
            IMapperService mapperService)
            : base(navigationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _coursesService = coursesService;
            _mapperService = mapperService;

            CurrentState = LayoutState.Loading;
        }

        #region -- Public properties --

        private ObservableCollection<CarouselBindableModel> _homeItems = new();
        public ObservableCollection<CarouselBindableModel> HomeItems
        {
            get => _homeItems;
            set => SetProperty(ref _homeItems, value);
        }

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await UpdateCoursesAsync();
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

        private async Task UpdateCoursesAsync()
        {
            CurrentState = LayoutState.Loading;

            var coursesResponse = await _coursesService.GetAllCoursesAsync();

            if (coursesResponse.IsSuccess)
            {
                var courses = await MapCoursesAsync(coursesResponse.Result);

                foreach (var group in courses.GroupBy(x => x.Category).OrderBy(x => x.Key))
                {
                    HomeItems.Add(new() { Items = new(group), Title = group.Key });
                }
            }

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

        private async Task<IEnumerable<CourseBindableModel>> MapCoursesAsync(IEnumerable<CourseModel> courses)
        {
            var mappedCourses = await _mapperService.MapRangeAsync<CourseBindableModel>(courses);

            foreach (var course in mappedCourses)
            {
                course.Users = new(await GetUsersForCourseAsync(course));
            }

            return mappedCourses;
        }

        private async Task<IEnumerable<UserBindableModel>> GetUsersForCourseAsync(CourseBindableModel course)
        {
            var result = Enumerable.Empty<UserBindableModel>();

            var users = await _userService.GetAllUsersAsync();

            if (users.IsSuccess)
            {
                var neededUsers = users.Result.Where(x => course.UsersIds.Contains(x.Id));
                result = await _mapperService.MapRangeAsync<UserBindableModel>(neededUsers);
            }

            return result;
        }

        #endregion
    }
}
