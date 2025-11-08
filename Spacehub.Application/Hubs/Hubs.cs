using Microsoft.AspNetCore.SignalR;

namespace SpaceHub.Application.Hubs;

public class NotifyHub : Hub
{
  public async Task SendSignal(string signal)
  {
    await Clients.All.SendAsync("ReceiveNotification", signal);
  }
}