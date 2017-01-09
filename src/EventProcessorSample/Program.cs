using System;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace EventProcessorSample
{
    public class Program
    {
        private const string EventHubConnectionString = "Endpoint=sb://dutchazuremeetupmlnk-eh-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=R+CNHd72m0lQ+UulRW9hkcKgp+CJUaFA4jZywSHIWm8=";
        private const string EventHubPath = "dambg";
        private const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dutchazuremeetupmlnkdev;AccountKey=f56kNpXufdHeSg9lLlkep1hNPOwCAF8xJoc/p8etttDRYS7pTsHrwxQiMtKvkAUs10O1G4h0g3HdWBbhuffW8w==;";
        private const string StorageLeaseContainerName = "eventprocessorhost";        

        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var eventProcessorHost = new EventProcessorHost(
                "dambg",
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageLeaseContainerName);

            IMessageLogger messageLogger = new ConsoleMessageLogger();

            IEventProcessorFactory processorFactory = new MyEventProcessorFactory(messageLogger);

            await eventProcessorHost.RegisterEventProcessorFactoryAsync(processorFactory);

            Console.WriteLine("Press enter key to exit...");
            Console.ReadLine();

            await eventProcessorHost.UnregisterEventProcessorAsync();        }
    }
}
