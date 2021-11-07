using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Producer
{
    public static class FanoutExchangePublisher
    {
        public static void Publish(IModel channel)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000}
            };

            channel.ExchangeDeclare(
                exchange: "demo-fanout-exchange",
                type: ExchangeType.Fanout,
                arguments: ttl); 

            var count = 0;

            while (true)
            {
                var message = new { Name = "Producer", Message = $"Hello! {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var propertise = channel.CreateBasicProperties();
                propertise.Headers = new Dictionary<string, object> { { "account", "new" } };

                channel.BasicPublish(
                    exchange: "demo-fanout-exchange",
                    routingKey: string.Empty,
                    basicProperties: propertise,
                    body: body);

                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
