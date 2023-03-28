using API.DataAccess.Repositories;
using API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;

        public ChatHub(IMessageRepository messageRepository)
        {
            _messageRepository= messageRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userEmail = Context.User.FindFirstValue("userName");

            await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userEmail = Context.User.FindFirstValue("userName");

            await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiver, string message)
        {
            var sender = Context.User.FindFirstValue("userName");

            await _messageRepository.AddMessage(sender, receiver, message, DateTimeOffset.Now.ToUnixTimeMicroseconds());

            await Clients.Group(receiver).SendAsync("ReceiveMessage", receiver, sender, message);
        }
    }
}
