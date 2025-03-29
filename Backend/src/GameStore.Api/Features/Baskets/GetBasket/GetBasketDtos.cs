namespace GameStore.Api.Features.Baskets.GetBasket;

public record class BasketDto(
    Guid CustomerId, 
    IEnumerable<BasketItemDto> Items
)
{
    public decimal TotalAmount => Items.Sum(x => x.Price * x.Quantity);
}

public record class BasketItemDto(
    Guid Id, 
    string name,
    decimal Price,
    int Quantity,
    string ImageUrl
);
