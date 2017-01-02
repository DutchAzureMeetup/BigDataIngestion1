using Amqp;
using Amqp.Framing;
using CommandLine;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ThermostatDataGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var commandLineArguments = (Parsed<Options>)Parser.Default.ParseArguments<Options>(args);

            Task t = MainAsync(commandLineArguments.Value);
            t.Wait();
        }

        static async Task MainAsync(Options options)
        {
            string namespaceUrl = $"{options.Namespace}.servicebus.windows.net";
            string policyName = options.PolicyName;
            string sasToken = WebUtility.UrlEncode(options.SasToken);

            string connectionString = $"amqps://{policyName}:{sasToken}@{namespaceUrl}/";

            Address address = new Address(connectionString);

            Connection connection = await Connection.Factory.CreateAsync(address);
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
                    CustomerId = options.CustomerId
                };

                string serializedJson = JsonConvert.SerializeObject(data);

                Message message = new Message
                {
                    BodySection = new Data {Binary = Encoding.UTF8.GetBytes(serializedJson)}
                };

                SenderLink sender = new SenderLink(session, "sender-link", options.EventHubName);

                await sender.SendAsync(message);

                await sender.CloseAsync();
            }
        }
    }

    class Options
    {
        [Option('n', "namespace", HelpText = "Name of the Namespace of the EventHub. Only the name (without .servicebus.windows.net)", Required = true)]
        public string Namespace { get; set; }

        [Option('p', "policyname", HelpText = "Name of the Policy which has Send rights on the Hub", Required = true)]
        public string PolicyName { get; set; }

        [Option('e', "eventhubname", HelpText = "Name of the Event Hub", Required = true)]
        public string EventHubName { get; set; }

        [Option('s', "sastoken", HelpText = "Sastoken to access to Event Hub", Required = true)]
        public string SasToken { get; set; }

        [Option('c', "customerid", HelpText = "CustomerId which you will find back in the data on the EventHub", Required = true)]
        public string CustomerId { get; set; }
    }

    class ThermostatData
    {
        public DateTime Date { get; set; }

        public int ElectricityUsage { get; set; }

        public string CustomerId { get; set; }
    }
}
