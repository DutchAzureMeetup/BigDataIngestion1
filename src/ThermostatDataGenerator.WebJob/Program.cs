using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amqp;
using Amqp.Framing;
using Newtonsoft.Json;

namespace ThermostatDataGenerator.WebJob
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Task t = MainAsync();
                t.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        static async Task MainAsync()
        {
            string customerId = "MarcoMansi";
            string nameSpace = String.Empty;
            string policyName = String.Empty;
            string eventHubName = String.Empty;
            string sasToken = WebUtility.UrlEncode(String.Empty);

            string namespaceUrl = $"{nameSpace}.servicebus.windows.net";
            string connectionString = $"amqps://{policyName}:{sasToken}@{namespaceUrl}/";

            Address address = new Address(connectionString);
            Connection connection;
            try
            {
                connection = await Connection.Factory.CreateAsync(address);
            }
            catch (Exception ex)
            {
                throw new Exception($"The namespace {nameSpace} is probably wrong.", ex);
            }

            Session session = new Session(connection);

            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            while (true)
            {
                DateTime date = DateTime.Now;
                Console.WriteLine($"{date} Generating data");

                ThermostatData data = new ThermostatData()
                {
                    Date = DateTime.Now,
                    ElectricityUsage = rnd.Next(0, 500),
                    CustomerId = customerId
                };

                string serializedJson = JsonConvert.SerializeObject(data);

                Message message = new Message
                {
                    BodySection = new Data { Binary = Encoding.UTF8.GetBytes(serializedJson) }
                };

                SenderLink sender = new SenderLink(session, "sender-link", eventHubName);

                try
                {
                    await sender.SendAsync(message);
                }
                catch (AmqpException ex)
                {
                    if (ex.Error.Condition.ToString().Contains("amqp:unauthorized-access"))
                    {
                        throw new Exception($"The policyname {policyName} or the SAS token {sasToken} is probably wrong.", ex);
                    }
                    else if (ex.Error.Condition.ToString().Contains("amqp:not-found"))
                    {
                        throw new Exception($"The eventhub name {eventHubName} is probably wrong.", ex);
                    }
                }

                await sender.CloseAsync();
            }

        }
    }

    class ThermostatData
    {
        public DateTime Date { get; set; }

        public int ElectricityUsage { get; set; }

        public string CustomerId { get; set; }
    }
}
