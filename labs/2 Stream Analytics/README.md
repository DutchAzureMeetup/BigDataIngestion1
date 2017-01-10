# Create an Azure Event Hub #

## Prerequisites: ##
- Azure subscription

## Let's start ##
In the following steps we are going to create with an ARM template all the resources needed to setup and communicate with the Stream Analytics.
The following resources will be created:

  * Storage Account
  * Event Hub
  * Service Bus with one Topic and one subscription
  * App Service Plan
  * App Service (with a webJob) for showing the output of the Service Bus 
  * A webjob to generate Thermostat event data and send it to the Event Hub (the same as the previous lab, but it's running     
  automatically in a WebJob)

## **Step 1** Deploy resources in Azure ##

Click the following button (hold CTRL while clicking to open in a new tab):

 <a target="_blank" id="deploy-to-azure"  href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FDutchAzureMeetup%2FBigDataIngestion1%2Fmaster%2Flabs%2F2%2520Stream%2520Analytics%2Fazuredeploy.json"><img src="http://azuredeploy.net/deploybutton.png"/></a>

And fill the forms: 

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/1.jpg?raw=true)

## **Step 2**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/2.jpg?raw=true)

## **Step 3**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/3.jpg?raw=true)

## **Step 4**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/4.jpg?raw=true)

## **Step 5**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/5.jpg?raw=true)

## **Step 6**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/6.jpg?raw=true)

## **Step 7**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/7.jpg?raw=true)

## **Step 8**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/8.jpg?raw=true)

## **Step 9**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/9.jpg?raw=true)

## **Step 10**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/10.jpg?raw=true)

## **Step 11**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/11.jpg?raw=true)

## **Step 12**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/12.jpg?raw=true)

## **Step 13**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/13.jpg?raw=true)

## **Step 14**  ##

Easy for copy/paste in this step: 

  * thermostatingestionbloboutput
  * fromeventhub
  * thermostatdata/fdate={date}/{time}


![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/14.jpg?raw=true)

## **Step 15**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/15.jpg?raw=true)

## **Step 16**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/16.jpg?raw=true)

## **Step 17**  ##

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

## **Step 18**  ##

![](https://github.com/DutchAzureMeetup/BigDataIngestion1/blob/master/labs/2%20Stream%20Analytics/img/18.jpg?raw=true)
