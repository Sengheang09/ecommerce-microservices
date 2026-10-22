namespace user_service.Services
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync<T>(string topic, T message);
    }
}