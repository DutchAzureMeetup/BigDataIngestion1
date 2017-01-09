
using System;
using Microsoft.Azure.EventHubs.Processor;

namespace EventProcessorSample
{
    public class MyEventProcessorFactory : IEventProcessorFactory
    {
        private readonly IMessageLogger _messageLogger;

        public MyEventProcessorFactory(IMessageLogger messageLogger)
        {
            _messageLogger = messageLogger;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            Console.WriteLine($"Creating event processor for partition {context.PartitionId}.");

            return new MyEventProcessor(_messageLogger);
        }
    }
}