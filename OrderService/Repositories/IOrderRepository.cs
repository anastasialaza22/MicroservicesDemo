using OrderService.Models;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();

        Order GetOrderById(int id);

        void CreateOrder(Order order);

        void UpdateOrder(Order order);

        void DeleteOrder(Order order);

        bool SaveChanges();
    }
}