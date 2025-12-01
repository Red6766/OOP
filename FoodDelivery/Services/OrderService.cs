using System.Collections.Generic;
using System.Linq;
using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public class OrderService
    {
        private readonly List<Order> _orders;
        private readonly OrderNotifier _notifier;

        public OrderService()
        {
            _orders = new List<Order>();
            _notifier = new OrderNotifier();
            
            // Observer паттерн 2
            _notifier.Subscribe(new OrderLogger());
        }

        public void AddOrder(Order order)
        {
            _orders.Add(order);
            _notifier.NotifyOrderCreated(order);
        }

        // Builder паттерн 3
        public Order CreateAndAddOrder(Customer customer, OrderType type, IEnumerable<MenuItem> items = null)
        {
            var builder = new OrderBuilder().WithCustomer(customer);
            if (type == OrderType.Express) builder.AsExpress();
            if (items != null) builder.AddItems(items);
            var order = builder.Build();
            AddOrder(order);
            return order;
        }

        public Order GetOrder(string orderId) => _orders.FirstOrDefault(o => o.Id == orderId);

        // Template Method паттерн 4
        public decimal CalculateOrderTotal(string orderId)
        {
            var order = GetOrder(orderId);
            if (order == null) return 0;
            OrderCalculator calculator = order.Type == OrderType.Express
                ? new ExpressOrderCalculator()
                : new StandardOrderCalculator();

            return calculator.CalculateTotal(order);
        }

        public void UpdateOrderStatus(string orderId, OrderStatus newStatus)
        {
            var order = GetOrder(orderId);
            if (order != null)
            {
                var oldStatus = order.Status;
                order.UpdateStatus(newStatus);
                _notifier.NotifyStatusChanged(order, oldStatus, newStatus);
            }
        }

        public bool AddItemToOrder(string orderId, MenuItem item)
        {
            var order = GetOrder(orderId);
            if (order != null && order.Status == OrderStatus.Created)
            {
                order.AddItem(item);
                _notifier.NotifyItemAdded(order, item);
                return true;
            }
            return false;
        }

        public List<Order> GetOrdersByStatus(OrderStatus status) =>
            _orders.Where(o => o.Status == status).ToList();

        public List<Order> GetAllOrders() => 
            new List<Order>(_orders.OrderByDescending(o => o.CreatedAt));
    }
}