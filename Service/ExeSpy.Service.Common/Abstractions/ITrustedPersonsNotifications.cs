using System.Threading.Tasks;
using EyeSpy.Service.Common.Models;

namespace EyeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsNotifications
    {
        Task<bool> SendDetectionNotificationAsync<T>(T notification) where T : BaseNotification, new();
    }
}