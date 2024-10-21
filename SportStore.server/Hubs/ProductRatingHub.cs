using Microsoft.AspNetCore.SignalR;
using SportStore.server.Requests;

namespace SportStore.server.Hubs;

public class ProductRatingHub : Hub
{
    public async Task NotifyUsersRatingChanged(ProductRating rating)
    {
        await Clients.All.SendAsync("ReceiveRating", rating);
    }
}