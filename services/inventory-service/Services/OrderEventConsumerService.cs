using System.Text.Json;
using Confluent.Kafka;
using inventory_service.DTOs.Request;
using inventory_service.Events;

namespace inventory_service.Services
{
    public class OrderEventConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderEventConsumerService> _logger;
        private readonly IConfiguration _configuration;

        public OrderEventConsumerService(
            IServiceProvider serviceProvider,
            ILogger<OrderEventConsumerService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bootstrapServers = _configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = "inventory-service-group", 
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            await Task.Yield(); 

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("order-placed-topic");

            _logger.LogInformation("[Kafka Consumer] Subscribed to topic 'order-placed-topic'. Waiting for events...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult?.Message?.Value != null)
                    {
                        var orderEvent = JsonSerializer.Deserialize<OrderPlacedEvent>(consumeResult.Message.Value);
                        if (orderEvent != null)
                        {
                            _logger.LogInformation($"[Kafka Consumer] Received OrderPlacedEvent for Order #{orderEvent.OrderNumber} (ID: {orderEvent.OrderId})");

                            using var scope = _serviceProvider.CreateScope();
                            var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();

                            var reserveDto = new ReserveStockDto
                            {
                                OrderId = orderEvent.OrderId,
                                ExpirationMinutes = 15,
                                Items = orderEvent.Items.Select(i => new ReserveStockItemDto
                                {
                                    ProductVariantId = i.ProductVariantId,
                                    Quantity = i.Quantity
                                }).ToList()
                            };

                            await inventoryService.ReserveStockAsync(reserveDto);
                            _logger.LogInformation($"[Kafka Consumer] Successfully RESERVED stock for Order #{orderEvent.OrderNumber}!");
                        }
                    }
                }
                
                catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                {
                    _logger.LogWarning("[Kafka Consumer] Topic 'order-placed-topic' is not created yet. Retrying in 3 seconds...");
                    await Task.Delay(3000, stoppingToken);
                }

                catch (OperationCanceledException)
                {
                    break;
                }

                catch (Exception ex)
                {
                    _logger.LogError($"[Kafka Consumer] Error processing message: {ex.Message}");
                    await Task.Delay(3000, stoppingToken);
                }
            }
        }
    }
}