using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace ServiceBusUI.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration("DefaultEndpointsProtocol=https;AccountName=dutchazuremeetupmlnkdev;AccountKey=f56kNpXufdHeSg9lLlkep1hNPOwCAF8xJoc/p8etttDRYS7pTsHrwxQiMtKvkAUs10O1G4h0g3HdWBbhuffW8w==")
            {
                // Use a custom NameResolver to get topic/subscription names from config.
                NameResolver = new ConfigurationNameResolver()
            };

            config.UseServiceBus(new Microsoft.Azure.WebJobs.ServiceBus.ServiceBusConfiguration()
            {
                ConnectionString = "Endpoint=sb://dutchazuremeetupmlnk-sb-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=GdQSx2qjUcj4SlDMz+cwMlaVZtxMTZG3xfu3lHtZbLA="
            });

            //if (config.IsDevelopment)
            //{
            //    config.UseDevelopmentSettings();
            //}

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
