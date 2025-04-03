using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public interface IBasketClient
{
    Task<CustomerBasket> GetBasketAsync(string customerId);

    Task<CommandResult> UpdateBasketAsync(CustomerBasket updatedBasket);
}
