using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public static class HeaderExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: "demo-header-exchange",
                type: ExchangeType.Headers);

            channel.QueueDeclare(
                queue: "demo-header-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header = new Dictionary<string, object> { { "account", "new" } };

            channel.QueueBind(
                queue: "demo-header-queue",
                exchange: "demo-header-exchange",
                routingKey: string.Empty,
                arguments: header);

            channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 10,
                global: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message);
            };

            channel.BasicConsume(
                queue: "demo-header-queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("consume started");
            Console.ReadLine();
        }
    }
}
