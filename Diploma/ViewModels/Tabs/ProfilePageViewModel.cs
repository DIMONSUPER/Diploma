using System.ComponentModel;
using Diploma.Services.Settings;
using Prism.Navigation;

namespace Diploma.ViewModels.Tabs
{
    public class ProfilePageViewModel : BaseTabViewModel
    {
        private readonly ISettingsManager _settingsManager;

        public ProfilePageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
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

        private bool _shouldNotifyMe;
        public bool ShouldNotifyMe
        {
            get => _shouldNotifyMe;
            set => SetProperty(ref _shouldNotifyMe, value);
        }

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SetUserInformation();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName is nameof(ShouldNotifyMe))
            {
                SetNotification();
            }
        }

        #endregion

        #region -- Private helpers --

        private void SetNotification()
        {
            _settingsManager.UserSettings.ShouldNotifyMe = ShouldNotifyMe;
        }

        private void SetUserInformation()
        {
            FirstName = "Dima";
            LastName = "Fedchenko";
            Description = "Fourth-year student of Dnypro National University";
            ShouldNotifyMe = _settingsManager.UserSettings.ShouldNotifyMe;
        }

        #endregion
    }
}
