using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    public static class TopicExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange:"demo-topic-exchange",
                type:ExchangeType.Topic);

            channel.QueueDeclare(
                queue: "demo-topic-queue",
                durable:true,
                exclusive:false,
                autoDelete:false,
                arguments:null);

            channel.QueueBind(
                queue:"demo-topic-queue",
                exchange:"demo-topic-exchange",
                routingKey:"account.*",
                arguments:null);

            channel.BasicQos(
                prefetchSize:0,
                prefetchCount:10,
                global:false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message);
            };

            channel.BasicConsume(
                queue: "demo-topic-queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("consume started");
            Console.ReadLine();
        }
    }
}
