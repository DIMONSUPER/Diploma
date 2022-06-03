using Prism.Events;
using Prism.Navigation;

namespace Diploma.ViewModels.Tabs
{
    public class NotificationsPageViewModel : BaseTabViewModel
    {
        public NotificationsPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator)
            : base(navigationService, eventAggregator)
        {
        }
    }
}
