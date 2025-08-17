using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
        void SubscribeToPlatformEvents();
        void UnsubscribeFromPlatformEvents();
    }
}