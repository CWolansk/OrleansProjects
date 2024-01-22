using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Runtime;
using Project1.DomainEvents;

namespace Project1.Grains
{
    public interface IDeviceGrain : IGrainWithStringKey
    {
        Task ScheduleUpdate(DateTimeOffset dateTime);

        Task EditScheduledUpdate(DateTimeOffset dateTime);

        Task<DeviceGrainState> GetState();

        Task<List<DomainEventBase>> GetEvents();
    }

    [StorageProvider(ProviderName = "OrleansLocalStorage")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class DeviceGrain: JournaledGrain<DeviceGrainState, DomainEventBase>, IDeviceGrain
    {
        public async Task<List<DomainEventBase>> GetEvents()
        {
            var events = await this.RetrieveConfirmedEvents(0, Version);

            var listOfEvents = events.ToList();

            return listOfEvents;
        }

        public async Task<DeviceGrainState> GetState()
        {
            return this.State;
        }

        public async Task ScheduleUpdate(DateTimeOffset dateTime)
        {
            RaiseEvent(new DeviceUpdateScheduledEvent
            {
                DeviceId = IdentityString,
                scheduledDateTime = dateTime,
                Version = Version + 1,
                TimeStamp = DateTimeOffset.UtcNow
            });

            await ConfirmEvents();
        }

        public async Task EditScheduledUpdate(DateTimeOffset dateTime)
        {
            RaiseEvent(new DeviceUpdateScheduleEditedEvent
            {
                DeviceId = IdentityString,
                editedScheduledDateTime = dateTime,
                Version = Version + 1,
                TimeStamp = DateTimeOffset.UtcNow
            });

            await ConfirmEvents();
        }

    }
}
