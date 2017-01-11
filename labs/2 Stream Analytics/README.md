# Create an Azure Event Hub

## **Prerequisites:**
- Azure subscription

## **Description**
In this lab we will create a Stream Analytics Job.

A Webjob is creating events like electricity usage and sending them to an Event Hub. 

The Stream Analytics Job will pull the events from the Event Hub (input) and as output:
  * save the events in a Blob Storage Account in a CSV file. 
  * every 5 seconds send the average electricity usage of the last 10 second to a Service Bus  

Then an App Service will show the events from the Service Bus in a web interface.

## **Let's start**
In the following steps we are going to create with an ARM template all the resources needed to setup and communicate with the Stream Analytics.  
The following resources will be created:

  * Storage Account
  * Event Hub
  * Service Bus with one Topic and one subscription
  * App Service Plan
  * App Service (with a webJob) for showing the output of the Service Bus 
  * A webjob to generate Thermostat event data and send it to the Event Hub (the same as the previous lab, but it's running     
  automatically in a WebJob)

## **Step 1** Deploy resources in Azure

__Delete the Resource Group created in the previous lab.__

Click the following button (hold CTRL while clicking to open in a new tab):

 <a target="_blank" id="deploy-to-azure"  href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FDutchAzureMeetup%2FBigDataIngestion1%2Fmaster%2Flabs%2F2%2520Stream%2520Analytics%2Fazuredeploy.json"><img src="http://azuredeploy.net/deploybutton.png"/></a>

And fill the forms: 

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/1.jpg?raw=true)

## **Step 2**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/2.jpg?raw=true)

## **Step 3**

The deployment will take around 5 minutes

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/3.jpg?raw=true)

## **Step 4**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/4.jpg?raw=true)

## **Step 5**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/5.jpg?raw=true)

## **Step 6**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/6.jpg?raw=true)

## **Step 7**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/7.jpg?raw=true)

## **Step 8**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/8.jpg?raw=true)

## **Step 9**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/9.jpg?raw=true)

## **Step 10**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/10.jpg?raw=true)

## **Step 11**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/11.jpg?raw=true)

## **Step 12**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/12.jpg?raw=true)

## **Step 13**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/13.jpg?raw=true)

## **Step 14**

Easy for copy/paste in this step: 

  * thermostatingestionbloboutput
  * fromeventhub
  * thermostatdata/fdate={date}/{time}


![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/14.jpg?raw=true)

## **Step 15**

Easy for copy/paste in this step: 

  * thermostatingestionservicebusoutput
  * webapp

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/15.jpg?raw=true)

## **Step 16**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/16.jpg?raw=true)

## **Step 17**

Copy and paste the following query:
```SQL
SELECT *
INTO   thermostatingestionbloboutput
FROM   thermostatingestioninput TIMESTAMP BY Date
PARTITION BY PartitionId

SELECT CustomerId, COUNT(*) AS Count, AVG(ElectricityUsage)
INTO   thermostatingestionservicebusoutput
FROM   thermostatingestioninput TIMESTAMP BY Date
GROUP BY CustomerId, HoppingWindow(second, 10 , 5)
```
![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/17.jpg?raw=true)

## **Step 18**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/18.jpg?raw=true)

In few minutes the job will start

## The Stream Analytics Job is created!

We have created a Stream Analytics Job, now let's check the outputs. 

# Check the output sent to the Service Bus

Go back to the Resource Group: 

## **Step 19**

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/19.png?raw=true)

## **Step 20**

Here we see the messages sent from Stream Analytics to the Service Bus Topic.
The App Service is taking the messages with a WebJob and visualize them in HTML. 

Every 5 second a message will appear with the average electricity consumption of the past 10 seconds.  
The messages are crated in Stream Analytics (see the previous query).

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/20.jpg?raw=true)

# Check the output sent to Blob Storage

## **Step 21** 
Go back to the Resource Group: 

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/21.jpg?raw=true)

## **Step 22** 

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/22.png?raw=true)

In the folder thermostatdata a new foder will be created with this format:

fdate=YYYY-MM-DD and in this folder a subfolder will be created for every hour.

Finally you will see two CSV files.

### Why two?    
The Event Hub where Stream Analytics is getting the event from has two partitions. 
The **PARTITION BY PartitionId** in the first query in the Stream Analytics job is writing a file for each Event Hub partition.

## **Step 23**

You can download the files craeted: 

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/23.png?raw=true)

Now open the file with your favorite Text Editor, you will see the data formatted as specified in the Blob Output of the Stream Analytics job.

There will be three extra properties added to the events which are not coming from the WebJob generator:

* EventProcessedUtcTime
* PartitionId
* EventEnqueuedUtcTime

This properties are added to the events by the Event Hub and they are in the files because we use a __SELECT *__ in the Stream Analytics Job's query. 