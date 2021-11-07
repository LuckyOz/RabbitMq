using RabbitMQ.Client;

namespace Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //QueueProducer.Publish(channel);
            //DirectExchangePublisher.Publish(channel);
            //TopicExchangePublisher.Publish(channel);
            //HeaderExchangePublisher.Publish(channel);
            FanoutExchangePublisher.Publish(channel);
        }
    }
}
