using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;

namespace Diploma.ViewModels
{
    public class BaseViewModel : BindableBase, IPageLifecycleAware, IApplicationLifecycleAware, IInitialize, IInitializeAsync, IDestructible
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Protected properties --

        protected INavigationService NavigationService { get; }

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

        public virtual void Destroy() { }

        #endregion
    }
}
