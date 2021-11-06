using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    public static class DirectExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: "demo-direct-exchange",
                type: ExchangeType.Direct);

            channel.QueueDeclare(
                queue: "demo-direct-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(
                queue: "demo-direct-queue",
                exchange: "demo-direct-exchange",
                routingKey: "account.init");

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
                queue: "demo-direct-queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("consume started");
            Console.ReadLine();
        }
    }
}
