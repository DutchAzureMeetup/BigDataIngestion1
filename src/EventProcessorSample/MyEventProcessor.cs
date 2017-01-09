using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace EventProcessorSample
{
    public class MyEventProcessor : IEventProcessor
    {
        private readonly IMessageLogger _messageLogger;

        public MyEventProcessor(IMessageLogger messageLogger)
        {
            _messageLogger = messageLogger;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Opened processor for partition {context.PartitionId}.");
            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Closed processor for partition {context.PartitionId}, reason: {reason}.");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            _messageLogger.LogMessages(messages);

            return context.CheckpointAsync();
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error occured in processor for partition {context.PartitionId}: {error}.");
            return Task.CompletedTask;
        }
    }
}