namespace GameStore.Frontend.Models;

public class CustomerBasket
{
    public required Guid CustomerId { get; set; }

    public List<BasketItem> Items { get; set; } = [];

    public decimal TotalAmount { get; set; }
}
