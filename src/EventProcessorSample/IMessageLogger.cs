using System.Collections.Generic;
using Microsoft.Azure.EventHubs;

namespace EventProcessorSample
{
    public interface IMessageLogger
    {
        void LogMessages(IEnumerable<EventData> messages);
    }
}