using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Course;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;

namespace Diploma.ViewModels.Tabs
{
    public class SearchPageViewModel : BaseTabViewModel
    {
        private readonly ICoursesService _coursesService;

        public SearchPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ICoursesService coursesService)
            : base(navigationService, eventAggregator)
        {
            _coursesService = coursesService;

            CurrentState = LayoutState.None;
        }

        #region -- Public properties --

        private string _currentQuery;
        public string CurrentQuery
        {
            get => _currentQuery;
            set => SetProperty(ref _currentQuery, value);
        }

        private ObservableCollection<CourseBindableModel> _searchResults = new();
        public ObservableCollection<CourseBindableModel> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand => _searchCommand ??= SingleExecutionCommand.FromFunc(OnSearchCommandAsync);

        #endregion

        #region -- Overrides --


        #endregion

        #region -- Private helpers --

        private async Task OnSearchCommandAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentQuery))
            {
                return;
            }

            CurrentState = LayoutState.Loading;

            var coursesResponse = await _coursesService.GetAllCoursesAsync();

            if (coursesResponse.IsSuccess)
            {
                var upperCuerry = CurrentQuery.ToUpper();

                var bindableCourses = await _coursesService.ConvertToBindableCourses(coursesResponse.Result);

                var queryCourses = bindableCourses.Where(x =>
                x.Category.ToUpper().Contains(upperCuerry) ||
                x.CreatedAt.ToString().ToUpper().Contains(upperCuerry) ||
                x.Description.ToUpper().Contains(upperCuerry) ||
                x.Language.ToUpper().Contains(upperCuerry) ||
                x.Name.ToUpper().Contains(upperCuerry) ||
                x.Teacher.Name.ToUpper().Contains(upperCuerry) ||
                x.Teacher.Description.ToUpper().Contains(upperCuerry) ||
                x.Teacher.Surname.ToUpper().Contains(upperCuerry) ||
                x.Teacher.Username.ToUpper().Contains(upperCuerry));

                SearchResults = new(queryCourses);
            }

            if (SearchResults.Any())
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

        #endregion
    }
}
