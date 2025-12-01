namespace FoodDelivery.Models
{
    public enum OrderStatus
    {
        Created,
        Preparing,
        ReadyForDelivery,
        OutForDelivery,
        Delivered,
        Cancelled
    }
}