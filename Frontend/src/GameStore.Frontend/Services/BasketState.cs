using System.IdentityModel.Tokens.Jwt;
using GameStore.Frontend.Clients;
using GameStore.Frontend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GameStore.Frontend.Services;

public class BasketState(
    IBasketClient client,
    AuthenticationStateProvider authenticationStateProvider)
{
    private EventCallback OnBasketUpdated { get; set; }

    private Task<CustomerBasket>? cachedBasket;

    public Task<CustomerBasket> GetBasketAsync()
    {
        return cachedBasket ??= FetchBasketAsync();
    }

    public async Task<CommandResult> AddItemAsync(BasketItem newItem)
    {
        var basket = await GetBasketAsync();

        basket.Items.Add(newItem);

        var result = await client.UpdateBasketAsync(basket);

        if (result.Succeeded)
        {
            // Invalidate the cache
            cachedBasket = null;

            // Notify subscriber
            await OnBasketUpdated.InvokeAsync();
        }

        return result;
    }

    public async Task<CommandResult> UpdateQuantityAsync(Guid id, int quantity)
    {
        var basket = await GetBasketAsync();

        var basketItem = basket.Items.Find(item => item.Id == id);

        if (basketItem is null)
        {
            // We will let the caller re-fetch the basket
            return new CommandResult(true);
        }

        basketItem.Quantity = quantity;

        var result = await client.UpdateBasketAsync(basket);

        if (result.Succeeded)
        {
            // Invalidate the cache
            cachedBasket = null;

            // Notify subscriber
            await OnBasketUpdated.InvokeAsync();
        }

        return result;
    }

    public async Task<CommandResult> RemoveItemAsync(Guid itemId)
    {
        var basket = await GetBasketAsync();

        basket.Items.RemoveAll(item => item.Id == itemId);

        var result = await client.UpdateBasketAsync(basket);

        if (result.Succeeded)
        {
            // Invalidate the cache
            cachedBasket = null;

            // Notify subscriber
            await OnBasketUpdated.InvokeAsync();
        }

        return result;
    }

    public void NotifyOnBasketUpdated(EventCallback callback)
    {
        OnBasketUpdated = callback;
    }

    private async Task<CustomerBasket> FetchBasketAsync()
    {
        var userId = await GetUserIdAsync();

        if (userId is null)
        {
            return new CustomerBasket() { CustomerId = Guid.Empty };
        }

        return await client.GetBasketAsync(userId);
    }

    private async Task<string?> GetUserIdAsync()
    {
        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();

        return authenticationState?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    }
}
