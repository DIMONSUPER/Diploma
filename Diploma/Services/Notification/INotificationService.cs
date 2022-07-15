using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;

namespace Diploma.Services.Notification
{
    public interface INotificationService
    {
        Task<AOResult<IEnumerable<NotificationModel>>> GetAllNotifications();
    }
}
