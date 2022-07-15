using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Diploma.Helpers;
using Diploma.Models;
using Prism.Events;
using Prism.Navigation;

namespace Diploma.ViewModels
{
    public class LessonPageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public LessonPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IUserDialogs userDialogs)
            : base(navigationService, eventAggregator)
        {
            _userDialogs = userDialogs;
        }

        #region -- Public properties --

        private LessonBindableModel _currentLesson;
        public LessonBindableModel CurrentLesson
        {
            get => _currentLesson;
            set => SetProperty(ref _currentLesson, value);
        }

        private ICommand _completeButtonTappedCommand;
        public ICommand CompleteButtonTappedCommand => _completeButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnCompleteButtonTappedCommand);

        private ICommand _backButtonTappedCommand;
        public ICommand BackButtonTappedCommand => _backButtonTappedCommand ??= SingleExecutionCommand.FromFunc(OnBackButtonTappedCommandAsync);

        private Task OnBackButtonTappedCommandAsync()
        {
            return NavigationService.GoBackAsync();
        }

        private Task OnCompleteButtonTappedCommand()
        {
            return _userDialogs.AlertAsync("Congratulations, your score is 100%!");
        }

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(LessonBindableModel), out LessonBindableModel lesson))
            {
                CurrentLesson = lesson;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(CurrentLesson) && CurrentLesson is not null)
            {
                foreach (var task in CurrentLesson.Tasks)
                {
                    task.PropertyChanged += OnTaskPropertyChanged;
                }
            }
        }

        private void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        #endregion

        #region -- Private helpers --

        #endregion
    }
}
