using Microsoft.VisualBasic;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Streams;
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

    [ImplicitStreamSubscription("decog-eventhubns-eastus-kew")]
    [StorageProvider(ProviderName = "AzureSqlStorage")]
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

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            // Create a GUID based on our GUID as a grain
            var deviceId = this.GetPrimaryKeyString();

            // Get one of the providers which we defined in config
            var streamProvider = this.GetStreamProvider("my-stream-provider");

            // Get the reference to a stream
            var streamId = StreamId.Create("decog-eventhubns-eastus-kew", deviceId);
            var stream = streamProvider.GetStream<object>(streamId);

            // Set our OnNext method to the lambda which simply prints the data.
            // This doesn't make new subscriptions, because we are using implicit 
            // subscriptions via [ImplicitStreamSubscription].

            await stream.SubscribeAsync<object>(
                async (data, token) =>
                {
                    //OnNext
                    RaiseEvent(new DeviceTwinChangeEvent
                    {
                        Id = this.GetPrimaryKeyString(),
                        Version = Version + 1,
                        TimeStamp = DateTimeOffset.UtcNow,
                        CurrentStreamSequenceToken = token,
                        Payload = data.ToString()
                    });

                    await ConfirmEvents();
                },
                (Exception) =>
                {
                    //OnError
                    return Task.CompletedTask;
                },
                () =>
                {
                    //OnCompleted
                    return Task.CompletedTask;
                }, //StreamToken
                    this.State?.CurrentStreamSequenceToken
                );
        }
    }
}
