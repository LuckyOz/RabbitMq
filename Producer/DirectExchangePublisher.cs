using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Producer
{
    public static class DirectExchangePublisher
    {
        public static void Publish(IModel channel)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000}
            };

            channel.ExchangeDeclare(
                exchange: "demo-direct-exchange", 
                type: ExchangeType.Direct, 
                arguments: ttl);

            var count = 0;

            while (true)
            {
                var message = new { Name = "Producer", Message = $"Hello{count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(
                    exchange: "demo-direct-exchange",
                    routingKey: "account.init",
                    body: body);

                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
