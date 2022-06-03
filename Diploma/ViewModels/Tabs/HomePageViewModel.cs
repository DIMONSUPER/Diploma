using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Events;
using Diploma.Models;
using Diploma.Services.Authorization;
using Diploma.Services.Course;
using Diploma.Services.Mapper;
using Diploma.Services.User;
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
                var courses = await _coursesService.ConvertToBindableCourses(coursesResponse.Result);

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

        private async void OnLanguageChanged(string language)
        {
            HomeItems = new();

            await UpdateCoursesAsync();
        }

        #endregion
    }
}
