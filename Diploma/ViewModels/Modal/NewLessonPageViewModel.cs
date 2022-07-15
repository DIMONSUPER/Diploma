using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class NewLessonPageViewModel : BaseViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;
        private readonly ICoursesService _coursesService;
        private int _currentCourseId;

        public NewLessonPageViewModel(
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

        private string _part;
        public string Part
        {
            get => _part;
            set => SetProperty(ref _part, value);
        }

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            set => SetProperty(ref _videoUrl, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private bool _isAddButtonEnabled;
        public bool IsAddButtonEnabled
        {
            get => _isAddButtonEnabled;
            set => SetProperty(ref _isAddButtonEnabled, value);
        }

        private ObservableCollection<TaskBindableModel> _tasks = new();
        public ObservableCollection<TaskBindableModel> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        private ICommand _addButtonTappedCommand;
        public ICommand AddButtonTappedCommand => _addButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnAddButtonTappedCommandAsync);

        private ICommand _addTaskButtonTappedCommand;
        public ICommand AddTaskButtonTappedCommand => _addTaskButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnAddTaskButtonTappedCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(CourseBindableModel), out CourseBindableModel course))
            {
                _currentCourseId = course.Id;
                Tasks.Add(new TaskBindableModel());
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(Part) or nameof(Title) or nameof(VideoUrl) or nameof(Description))
            {
                IsAddButtonEnabled = !string.IsNullOrWhiteSpace(Part)
                    && !string.IsNullOrWhiteSpace(Title)
                    && !string.IsNullOrWhiteSpace(VideoUrl)
                    && !string.IsNullOrWhiteSpace(Description)
                    && Tasks.Any();
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnAddTaskButtonTappedCommandAsync()
        {
            Tasks.Add(new());

            return Task.CompletedTask;
        }

        private Task OnBackButtonTappedCommandAsync()
        {
            return NavigationService.GoBackAsync();
        }

        private async Task OnAddButtonTappedCommandAsync()
        {
            if (IsInternetConnected)
            {
                foreach (var task in Tasks)
                {
                    task.PossibleAnswers = new(task.PossibleAnswersText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
                }

                var newLesson = new LessonBindableModel
                {
                    Title = Title,
                    Description = Description,
                    VideoUrl = VideoUrl,
                    Part = int.Parse(Part),
                    Tasks = Tasks,
                };

                var createNewLessonResponse = await _coursesService.PostNewLessonForCourseAsync(newLesson, _currentCourseId);

                if (createNewLessonResponse.IsSuccess)
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
