namespace GameStore.Frontend.Models;

public class BasketItem
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public required string ImageUri { get; set; }
}
