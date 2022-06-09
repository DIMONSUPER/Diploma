using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace Diploma.ViewModels
{
    public class BaseViewModel : BindableBase, IPageLifecycleAware, IApplicationLifecycleAware, IInitialize, IInitializeAsync, IDestructible, INavigatedAware
    {
        public BaseViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator)
        {
            NavigationService = navigationService;
            EventAggregator = eventAggregator;

            Connectivity.ConnectivityChanged += OnConnectionChanged;
        }

        #region -- Public properties --

        public bool IsInternetConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        private LayoutState _currentState;
        public LayoutState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }

        #endregion

        #region -- Protected properties --

        protected INavigationService NavigationService { get; }

        protected IEventAggregator EventAggregator { get; }

        #endregion

        #region -- IPageLifecycleAware implementation --

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        #endregion

        #region -- IApplicationLifecycleAware implementation --

        public virtual void OnResume() { }

        public virtual void OnSleep() { }

        #endregion

        #region -- IInitialize implementation --

        public virtual void Initialize(INavigationParameters parameters) { }

        #endregion

        #region -- IInitializeAsync implementation --

        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;

        #endregion

        #region -- IDestructible implementation --

        public virtual void Destroy()
        {
            Connectivity.ConnectivityChanged -= OnConnectionChanged;
        }

        #endregion

        #region -- INavigatioAware implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        #endregion

        #region -- Protected helpers --

        protected virtual void OnConnectionChanged(object sender, ConnectivityChangedEventArgs e) { }

        #endregion
    }
}
