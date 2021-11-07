using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public static class FanoutExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: "demo-fanout-exchange",
                type: ExchangeType.Fanout);

            channel.QueueDeclare(
                queue: "demo-fanout-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(
                queue: "demo-fanout-queue",
                exchange: "demo-fanout-exchange",
                routingKey: string.Empty);

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
                queue: "demo-fanout-queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("consume started");
            Console.ReadLine();
        }
    }
}
