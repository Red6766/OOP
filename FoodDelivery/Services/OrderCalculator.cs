using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public abstract class OrderCalculator
    {
        public decimal CalculateTotal(Order order)
        {
            var basePrice = CalculateBasePrice(order);
            var deliveryFee = CalculateDeliveryFee(order);
            var discount = CalculateDiscount(order);
            return basePrice + deliveryFee - discount;
        }

        protected virtual decimal CalculateBasePrice(Order order) => 
            order.Items.Sum(item => item.Price);
        
        protected abstract decimal CalculateDeliveryFee(Order order);
        protected virtual decimal CalculateDiscount(Order order) => 0;
    }

    public class StandardOrderCalculator : OrderCalculator
    {
        protected override decimal CalculateDeliveryFee(Order order) => 2.99m;
    }

    public class ExpressOrderCalculator : OrderCalculator
    {
        protected override decimal CalculateDeliveryFee(Order order) => 5.99m;
    }
}