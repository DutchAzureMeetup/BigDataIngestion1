
# Azure Event Hub Processor for .NET Core

## Prerequisites 

Download the .NET Core 1.0.3 SDK for your environment from https://www.microsoft.com/net/download/core:

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/3%20Event%20Processor/img/downloaddotnetcore.png?raw=true)

## Let's start

- Open the command prompt (or terminal) and create a new folder for the C# project.

- Navigate to the folder

- Add a file called global.json with the following content and save the file:

```json
{
    "sdk": {
        "version": "1.0.0-preview2-003156"
    }
}

``` 

- Type ```dotnet new``` to generate a sample program.

To use the Event Hubs Processor Host we'll need some references to preview packages. Add the following dependencies to project.json:

```json
    "dependencies": {
        "Microsoft.Azure.EventHubs": "0.0.4-preview",
        "Microsoft.Azure.EventHubs.Processor": "0.0.4-preview"
    },
```

- Now run ```dotnet restore``` to resolve all dependencies. Note that the restore will fail because not all dependencies are compatible with ```netcoreapp1.0```.
The reason for this error is that the ```Microsoft.Azure.EventHubs``` packages are dependent on packages that only support the full .NET Framework or .NET Portable.
To get rid of this error, we must configure the .NET Core application to allow for .NET Portable dependencies.
Update the imports element to include ```portable-net45+win8```:

```json
    "imports": [
        "dnxcore50",
        "portable-net45+win8"
    ]
```

- Run ```dotnet restore``` again which should now succeed.

- Add a new ```MyEventProcessor.cs``` file to the project. In it, create a ```MyEventProcessor``` class which implements the ```IEventProcessor``` interface.
The ```MyEventProcessor``` class will receive the messages from the Event Hub and allow us to process them however we want.

- The ```IEventProcessor``` contains four methods to implement.
```OpenAsync``` and ```CloseAsync``` are called when a processor is opened or closed for a specific Event Hub partition, respectively.
```ProcessErrorAsync``` is called when any exception has occured in the processor, and ```ProcessEventsAsync``` will be called with the actual batch of messages to process.
Implement the ```OpenAsync```, ```CloseAsync``` and ```ProcessErrorAsync``` methods by simply logging the call and returning a completed ```Task```:

```C#
    public class MyEventProcessor : IEventProcessor
    {
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

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error occured in processor for partition {context.PartitionId}: {error}.");
            return Task.CompletedTask;
        }
    }
```

- Add an implementation for the ```ProcessEventsAsync``` method.
Again, since this is a sample app we're simply logging the messages to the console.
After logging the messages call ```context.CheckpointAsync``` to let the processor host know that we're done with this message batch.
This ensures that the messages won't be processed again if the processor loses its lease to another processor.
However, since the application may crash or be stopped after message processing but before the call to ```CheckpointAsync``` is completed the application must still be written to support at-least-once messaging. 

```C#
    ...

    public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
    {
        Console.WriteLine($"Received {messages.Count()} message(s) from partition {context.PartitionId}.");

        return context.CheckpointAsync();
    }

    ...
```

- To use this processor, we need to register it with a ```EventProcessorHost```.
Before we can create an ```EventProcessorHost``` we'll need some connection strings though.

    - ```eventHubName```: Name of the Event Hub to read from. If you've used the Deploy to Azure button in the previous lab, this will be ```dambg```.
    
    - ```eventHubConnectionString```: In the Azure Portal browse to your Event Hubs namespace and select ```Shared access policies```.
    Click on the ```RootManageSharedAccessKey``` policy and in the new screen copy the connection string from the ```CONNECTION STRING-PRIMARY KEY``` field.

    - ```storageConnectionString```: In the Azure Portal browse to your Storage account and select ```Access keys```. Click on the ```...``` button behind the ```key1``` field to view the connection string.

- Now open Program.cs and add the following code to the ```Pogram``` class.
Fill in the placeholders with the values from the previous step.

```C#
    private static async Task MainAsync()
    {
        var eventProcessorHost = new EventProcessorHost(
            "<eventHubName>",
            PartitionReceiver.DefaultConsumerGroupName,
            "<eventHubConnectionString>",
            "<storageConnectionString>",
            "leasecontainer");

        // Register the processor with the host.
        // This will automatically start instances of the processor.
        await eventProcessorHost.RegisterEventProcessorAsync<MyEventProcessor>();

        Console.WriteLine("Press enter key to exit...");
        Console.ReadLine();

        // Unregister event processors to stop processing and release leases.
        await eventProcessorHost.UnregisterEventProcessorAsync();
    }
```

- Modify the original ```Main``` method to call the new ```MainAsync``` method:

```C#
    public static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();
    }
```

- Build the project by running ```dotnet build```.

- You can now run the project by running ```dotnet run```. You will see a processor being instantiated for each Event Hub partition.

## Dependency Injection

You may have noticed that you register the processor with the host by calling the generic ```RegisterEventProcessorAsync<T>``` method.
This registers the processor's type in the host so that it can create instances on demand.
However, if we're just registering the type, how can we inject any dependencies the processor may need?
To illustrate how this can be done, we'll first add a dependency to the ```MyEventProcessor```.

- Create a new ```IMessageLogger``` interface with a single method:

```C#
    public interface IMessageLogger
    {
        void LogMessages(IEnumerable<EventData> messages);
    }
```

- Create a new ```ConsoleMessageLogger``` class that implements the interface.
We will inject an instance of this class into the ```MyEventProcessor``` to log the messages.

```C#
    public class ConsoleMessageLogger : IMessageLogger
    {
        public void LogMessages(IEnumerable<EventData> messages)
        {
            Console.WriteLine($"Message logger received {messages.Count()} message(s).");
        }
    }
```

- Add a constructor to the ```MyEventProcessor``` class that takes an instance of ```IMessageLogger``` and stores it in a private field:

```C#
    private readonly IMessageLogger _messageLogger;

    public MyEventProcessor(IMessageLogger messageLogger)
    {
        _messageLogger = messageLogger;
    }
```

- Modify the ```ProcessEventsAsync``` method to use the injected ```IMessageLogger``` instead of directly writing to the console:

```C#
    public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
    {
        _messageLogger.LogMessages(messages);

        return context.CheckpointAsync();
    }
```

- Now that the ```MyEventProcessor``` requires a constructor argument we'll need a different way to register it with the host.
For this, we'll need a separate factory class that implements ```IEventProcessorFactory``` and can create instances of ```MyEventProcessor```:

```C#
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
```

- Modify the ```MainAsync``` method in Program.cs to register the factory instead of directly registering the processor:

```C#
    IMessageLogger messageLogger = new ConsoleMessageLogger();
    IEventProcessorFactory processorFactory = new MyEventProcessorFactory(messageLogger);

    await eventProcessorHost.RegisterEventProcessorFactoryAsync(processorFactory);
```

- Build and run the project again. The ```MyEventProcessor``` will now use the injected ```IMessageLogger``` to write the message count to the console.
