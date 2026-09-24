using user_service.Models;

namespace user_service.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByCustomerIdAsync(long customerId);
        Task<Order?> GetByIdAsync(long id);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task AddStatusHistoryAsync(OrderStatusHistory history);
    }
}
