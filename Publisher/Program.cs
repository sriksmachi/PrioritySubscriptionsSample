using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {

        const string TopicName = "orders-topic";
        const string ConnectionString = "Endpoint=sb://contoso-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=wbm9LHAOhYMeyfJfhiJcr4FFh/2Jjk6x//8/Dweii/U=";

        public static int Main(string[] args)
        {
            try
            {
                var program = new Program();
                program.SendMessagesToTopicAsync(ConnectionString).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
            return 0;
        }

        async Task SendMessagesToTopicAsync(string connectionString)
        {
            // Create client for the topic.
            var topicClient = new TopicClient(connectionString, TopicName);

            // Create a message sender from the topic client.

            Console.WriteLine("\nSending orders to topic. Press any key to Start...");

            Console.ReadKey();

            // Now we can start sending orders.
            await Task.WhenAll(
                SendOrder(topicClient, new Order()),
                SendOrder(topicClient, new Order { Color = "blue", Quantity = 5, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "red", Quantity = 10, Priority = "HIGH" }),
                SendOrder(topicClient, new Order { Color = "yellow", Quantity = 5, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "blue", Quantity = 10, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "blue", Quantity = 5, Priority = "HIGH" }),
                SendOrder(topicClient, new Order { Color = "blue", Quantity = 10, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "red", Quantity = 5, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "red", Quantity = 10, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "red", Quantity = 5, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "yellow", Quantity = 10, Priority = "HIGH" }),
                SendOrder(topicClient, new Order { Color = "yellow", Quantity = 5, Priority = "LOW" }),
                SendOrder(topicClient, new Order { Color = "yellow", Quantity = 10, Priority = "LOW" })
                );

            Console.WriteLine("All messages sent.");
        }

        async Task SendOrder(TopicClient topicClient, Order order)
        {
            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(order)))
            {
                CorrelationId = order.Priority,
                Label = order.Color,
                UserProperties =
                {
                    { "color", order.Color },
                    { "quantity", order.Quantity },
                    { "priority", order.Priority }
                }
            };

            await topicClient.SendAsync(message);

            Console.WriteLine("Sent order with Color={0}, Quantity={1}, Priority={2}", order.Color, order.Quantity, order.Priority);
        }
    }

    class Order
    {
        public string Color
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public string Priority
        {
            get;
            set;
        }
    }
}
