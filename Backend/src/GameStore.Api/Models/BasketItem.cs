namespace GameStore.Api.Models;

public class BasketItem
{
    public Guid Id { get; set; }

    public Game? Game { get; set; }

    public Guid GameId { get; set; }

    public int Quantity { get; set; }

    public Guid CustomerBasketId { get; set; }
}
