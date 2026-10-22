using System.Text.Json;
using Confluent.Kafka;

namespace user_service.Services.Impl
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;

        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All 
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync<T>(string topic, T message)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);
                var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
                _logger.LogInformation($"[Kafka Producer] Published event to topic '{topic}' at offset {result.Offset}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Kafka Producer] Failed to publish message: {ex.Message}");
            }
        }

    }
}