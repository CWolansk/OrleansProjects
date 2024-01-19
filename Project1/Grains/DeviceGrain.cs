using Orleans.EventSourcing;
using Orleans.Runtime;
using Project1.DomainEvents;

namespace Project1.Grains
{
    public interface IDeviceGrain : IGrainWithStringKey
    {

    }

    public class DeviceGrain: JournaledGrain<DeviceGrainState, DomainEventBase>, IDeviceGrain
    {
        private readonly IPersistentState<DeviceGrainState> _state;
        
        public DeviceGrain(
            [PersistentState(stateName: "device", storageName: "devices")] IPersistentState<DeviceGrainState> state )
        {
            _state = state;
        }

    }
}
