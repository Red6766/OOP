using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    // Factory Method паттерн 5
    public abstract class OrderFactory
    {
        public abstract Order CreateOrder(Customer customer);
    }

    public class StandardOrderFactory : OrderFactory
    {
        public override Order CreateOrder(Customer customer)
        {
            return new Order(customer, OrderType.Standard);
        }
    }

    public class ExpressOrderFactory : OrderFactory
    {
        public override Order CreateOrder(Customer customer)
        {
            return new Order(customer, OrderType.Express);
        }
    }
}