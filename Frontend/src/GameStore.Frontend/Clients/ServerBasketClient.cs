using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public partial class ServerBasketClient(HttpClient httpClient) : IBasketClient
{
    public async Task<CustomerBasket> GetBasketAsync(string customerId)
        => await httpClient.GetFromJsonAsync<CustomerBasket>($"baskets/{customerId}")
            ?? throw new Exception("Could not find basket!");

    public async Task<CommandResult> UpdateBasketAsync(CustomerBasket updatedBasket)
    {
        var dto = new UpdateBasketDto(updatedBasket.Items.Select(
            item => new UpdateBasketItemDto(item.Id, item.Quantity)).ToList());

        var response = await httpClient.PutAsJsonAsync($"baskets/{updatedBasket.CustomerId}", updatedBasket);
        return await response.HandleAsync();
    }
}

public record class UpdateBasketDto(IEnumerable<UpdateBasketItemDto> Items);

public record class UpdateBasketItemDto(Guid Id, int Quantity);