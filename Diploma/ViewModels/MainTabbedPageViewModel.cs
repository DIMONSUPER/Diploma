using Prism.Events;
using Prism.Navigation;

namespace Diploma.ViewModels
{
    public class MainTabbedPageViewModel : BaseViewModel
    {
        public MainTabbedPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator)
            : base(navigationService, eventAggregator)
        {
        }
    }
}
