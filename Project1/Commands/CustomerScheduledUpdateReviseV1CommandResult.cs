﻿using Project1.DomainEvents;
using Project1.Grains;

namespace Project1.Commands
{
    public class CustomerScheduledUpdateReviseV1CommandResult
    {
        public DeviceGrainState DeviceGrain { get; set; }
    }
}
