using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Producer
{
    public static class HeaderExchangePublisher
    {
        public static void Publish(IModel channel)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000}
            };

            channel.ExchangeDeclare(
                exchange: "demo-header-exchange",
                type: ExchangeType.Headers,
                arguments: ttl);

            var count = 0;

            while (true)
            {
                var message = new { Name = "Producer", Message = $"Hello! {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var propertise = channel.CreateBasicProperties();
                propertise.Headers = new Dictionary<string, object> { { "account", "new" } };

                channel.BasicPublish(
                    exchange: "demo-header-exchange",
                    routingKey: string.Empty,
                    basicProperties: propertise,
                    body: body);

                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
