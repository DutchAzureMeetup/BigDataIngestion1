using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.EventHubs;

namespace EventProcessorSample
{
    public class ConsoleMessageLogger : IMessageLogger
    {
        public void LogMessages(IEnumerable<EventData> messages)
        {
            Console.WriteLine($"Message logger received {messages.Count()} message(s).");
        }
    }
}