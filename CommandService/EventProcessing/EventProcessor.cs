using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;

        }

        public void ProcessEvent(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Received an empty message to process.");
                throw new ArgumentNullException(nameof(message));
            }

            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    // Handle platform published event
                    break;
                case EventType.Undetermined:
                default:
                    // Handle undetermined or unknown events
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine($"--> Determining Event for message: {notificationMessage}");

            if (string.IsNullOrEmpty(notificationMessage))
            {
                return EventType.Undetermined;
            }

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            if (eventType is null || string.IsNullOrEmpty(eventType.Event))
            {
                Console.WriteLine($"--> Could not determine event type for message: {notificationMessage}");
                return EventType.Undetermined;
            }

            switch (eventType.Event)
            {
                case "PlatformPublished":
                    Console.WriteLine($"--> Platform published event detected: {notificationMessage}");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine($"--> Unknown event type: {eventType.Event}");
                    return EventType.Undetermined;
            }
        }
        
        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);

                    if (!repo.ExternalPlatformExist(plat.ForeignId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChange();
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exist...");
                    }



                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
                    return;
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}