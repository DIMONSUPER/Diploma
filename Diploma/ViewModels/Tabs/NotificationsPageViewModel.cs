using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Models;
using Diploma.Services.Notification;
using Prism.Events;
using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;

namespace Diploma.ViewModels.Tabs
{
    public class NotificationsPageViewModel : BaseTabViewModel
    {
        private readonly INotificationService _notificationService;

        public NotificationsPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            INotificationService notificationService)
            : base(navigationService, eventAggregator)
        {
            _notificationService = notificationService;

            CurrentState = LayoutState.Loading;
        }

        #region -- Public properties --

        private ObservableCollection<NotificationModel> _notifications = new();
        public ObservableCollection<NotificationModel> Notifications
        {
            get => _notifications;
            set => SetProperty(ref _notifications, value);
        }

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await UpdateNotificationsAsync();
        }

        #endregion

        #region -- Private helpers --

        private async Task UpdateNotificationsAsync()
        {
            CurrentState = LayoutState.Loading;

            var notifications = await _notificationService.GetAllNotifications();

            if (notifications.IsSuccess)
            {
                Notifications = new(notifications.Result);
            }

            CurrentState = Notifications.Any() ? LayoutState.Success : LayoutState.Empty;
        }

        #endregion
    }
}
