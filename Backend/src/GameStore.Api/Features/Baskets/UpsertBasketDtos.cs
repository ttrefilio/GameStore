namespace GameStore.Api.Features.Baskets;

public record class UpsertBasketDto(IEnumerable<UpsertBasketItemDto> Items);

public record class UpsertBasketItemDto(Guid Id, int Quantity);

