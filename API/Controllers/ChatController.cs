using API.DataAccess.Repositories;
using API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public ChatController(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpGet, Route("messages")]
        public async Task<IActionResult> GetMessagesWithUser(int conversationPartnerId)
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));
            var messages = await _messageRepository.GetUsersMessages(userId.Value, conversationPartnerId);
            var messagesDto = new List<MessageDto>();

            foreach(var message in messages)
            {
                messagesDto.Add(new MessageDto
                {
                    SenderEmail = await _userRepository.GetUserEmail(message.FromUser) ?? "unknown",
                    ReceiverEmail = await _userRepository.GetUserEmail(message.ToUser) ?? "unknown",
                    Message = message.MessageText,
                    DateTime = DateTimeOffset.FromUnixTimeMilliseconds(message.SentTimestamp / 1000)
                });
            }

            messagesDto.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));

            return Ok(messagesDto);
        }

        [HttpGet, Route("conversationPartners")]
        public async Task<IActionResult> GetUserConversationPartners()
        {
            var userId = await _userRepository.GetUserId(User.FindFirstValue("userName"));

            var partnerIds = await _messageRepository.GetUsersMessagePartners(userId.Value);

            return Ok(partnerIds);
        }
    }
}
