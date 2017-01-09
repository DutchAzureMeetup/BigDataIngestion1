# Create an Azure Event Hub #

## Prerequisites: ##
- Azure subscription

## 0 Login to the Azure Portal ##

 [https://portal.azure.com](https://portal.azure.com) 
   

## 1  ##
   
![](https://raw.githubusercontent.com/DutchAzureMeetup/BigDataIngestion1/master/labs/1%20Azure%20Event%20Hub/img/01%20Logged%20in.PNG)

## 2  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/02%20Search%20Event%20Hubs.PNG?raw=true)

## 3  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/03%20Search%20Results.PNG?raw=true)

## 4  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/04%20Create%20Event%20Hubs.PNG?raw=true)

## 5  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/05%20Create%20Event%20Hubs%20Namespace.PNG?raw=true)

## 6  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/06%20Deployment%20status.PNG?raw=true)

## 7  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/07%20Deployment%20Succeeded.PNG?raw=true)

## 8  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/08%20Resource%20Groups.PNG?raw=true)

## 9  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/09%20Event%20Hubs%20selected.PNG?raw=true)

## 10  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/10%20Add%20Event%20Hub.PNG?raw=true)

## 11  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/11%20Create%20Event%20Hub.PNG?raw=true)

## 12  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/12%20Event%20Hub%20created.PNG?raw=true)

## 13  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/13%20Shared%20access%20policies.PNG?raw=true)

## 14  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/14%20Create%20shared%20access%20policy.PNG?raw=true)

## 15 Copy the Primary connectionstring##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/15%20Copy%20Connectionstring.png?raw=true)

    Copy the primary connectionstring, you will need it in a few moments.

You are finished creating the Azure Event Hubs, with an Event Hub and on the Event Hub a Listen only Access Policy.

Let's send some data

## 15 Get source from git repo##

Clone (or download) the source from here in your Git client:
[https://github.com/DutchAzureMeetup/BigDataIngestion1](https://github.com/DutchAzureMeetup/BigDataIngestion1)

## 16 ##

Open the bin folder in a command prompt

## 17 Start the Dotnet Core application  

Start the datagenerator by typing in:

    dotnet ThermostatDataGenerator.dll <PrimaryConnectionString>

So something like (use your own connectionstring!):

> dotnet ThermostatDataGenerator.dll Endpoint=sb://myuniquename.servicebus.windows.net/;SharedAccessKeyName=SendOnly;SharedAccessKey=yrxY0zUTLx/prfVBCs2I6OiZqOiW6qW6gwjUhJFbCmQ=;EntityPath=dambd

The datagenerator will start generating data with a generic customerId.
If you like to pass your own customerid than pass it as a second argument, like:

> dotnet ThermostatDataGenerator.dll Endpoint=sb://myuniquename.servicebus.windows.net/;SharedAccessKeyName=SendOnly;SharedAccessKey=yrxY0zUTLx/prfVBCs2I6OiZqOiW6qW6gwjUhJFbCmQ=;EntityPath=dambd pascalnaber

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/16%20Start%20Generator.PNG?raw=true)

## 18 Take a look at the graphs

Click on the Event Hubs name:

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/1%20Azure%20Event%20Hub/img/17%20See%20graphs.PNG?raw=true)

## If time permits

For Windows there is a tool called [ServiceBus Explorer](https://code.msdn.microsoft.com/windowsapps/Service-Bus-Explorer-f2abca5a).
Connect to the EventHub and take a look at the data.