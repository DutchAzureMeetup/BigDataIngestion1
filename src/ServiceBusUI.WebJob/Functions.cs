using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace ServiceBusUI.WebJob
{
    public class Functions
    {
        private static readonly HttpClient Client = new HttpClient();

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([ServiceBusTrigger("%topicName%", "%subscriptionName%")] string message, TextWriter log)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { Body = message }),
                Encoding.UTF8,
                "application/json");

            Client.PostAsync("/api/Message", content).GetAwaiter().GetResult();

            log.WriteLine(message);
        }
    }
}
