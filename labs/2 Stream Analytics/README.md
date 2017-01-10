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

## Step 1 Deploy resources in Azure ##

Click the following button (hold CTRL while clicking to open in a new tab):

 <a target="_blank" id="deploy-to-azure"  href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FDutchAzureMeetup%2FBigDataIngestion1%2Fmaster%2Flabs%2F2%2520Stream%2520Analytics%2Fazuredeploy.json"><img src="http://azuredeploy.net/deploybutton.png"/></a>

## Step 2  ##

  