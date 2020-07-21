using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Consumers
{
    public static class Function2
    {
        [FunctionName("LowPriorityMessageHandler")]
        public static void Run([ServiceBusTrigger("orders-topic", "lowprioritysubscription", 
            Connection = "ServiceBusConnection")]string mySbMsg,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
