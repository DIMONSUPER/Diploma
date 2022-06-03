using System;
using Prism;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.Helpers;

namespace Diploma.ViewModels.Tabs
{
    public class BaseTabViewModel : BaseViewModel, IActiveAware
    {
        private readonly DelegateWeakEventManager _isActiveChangedEventManager = new();

        public BaseTabViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator)
            : base(navigationService, eventAggregator)
        {
        }

        #region -- Protected methods --

        protected virtual void RaiseIsActiveChanged()
        {
            _isActiveChangedEventManager.RaiseEvent(this, EventArgs.Empty, nameof(IsActiveChanged));
        }

        #endregion

        #region -- IActiveAware implementation --

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public event EventHandler IsActiveChanged
        {
            add => _isActiveChangedEventManager.AddEventHandler(value);
            remove => _isActiveChangedEventManager.RemoveEventHandler(value);
        }

        #endregion
    }
}
