namespace GameStore.Api.Models;

public class CustomerBasket
{
    public Guid Id { get; set; }

    public List<BasketItem> Items { get; set; } = [];
}
