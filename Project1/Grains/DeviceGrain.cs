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

        [Alias("GetState")]
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

        public Task<DeviceGrainState> GetState() => Task.FromResult(this.State);

        public async Task ScheduleUpdate(DateTimeOffset dateTime)
        {
            RaiseEvent(new DeviceUpdateScheduledEvent
            {
                Id = this.GetPrimaryKeyString(),
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
                Id = this.GetPrimaryKeyString(),
                editedScheduledDateTime = dateTime,
                Version = Version + 1,
                TimeStamp = DateTimeOffset.UtcNow
            });

            await ConfirmEvents();
        }

    }
}
