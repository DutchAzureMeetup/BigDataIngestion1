[![Build Status](https://travis-ci.org/DutchAzureMeetup/BigDataIngestion1.svg?branch=master)](https://travis-ci.org/DutchAzureMeetup/BigDataIngestion1)


# BigDataIngestion1

## Deploy Azure Resources with ARM 

Please follow this steps to get everything working: 

1. Clone this repo: https://github.com/DutchAzureMeetup/BigDataIngestion1.git
2. In your favorite text editor open this file: \src\AzureInfrastructure\azuredeploy.parameters.json
3. Change the value of the "projectName" to something else and save the file (projectName must be between 3 and 21 characters in length and use numbers and lower-case letters only).
4. In a powershell prompt navigate to this directory: \src\AzureInfrastructure
5. Login in azure with your credentials: **Login-AzureRmAccount**
6. **[Optional]** Follow this only if you have multiple Azure subscriptions:

  6.1 Get a list of your subscriptions: **Get-AzureRmSubscription**  
  6.2 Select the subscription which you want to use: **Select-AzureRmSubscription -SubscriptionId** {put here your subscriptionid}
  
7. Deploy: **.\deploy.ps1**