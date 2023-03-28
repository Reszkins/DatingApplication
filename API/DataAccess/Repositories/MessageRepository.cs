using API.Models;
using System.Security.Principal;

namespace API.DataAccess.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessage(string senderEmail, string receiverEmail, string message, long timestamp);
        Task<List<Message>> GetUsersMessages(int userId, int conversationPartnerId);
        Task<List<int>> GetUsersMessagePartners(int userId);
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IUserRepository _userRepository;
        public MessageRepository(ISqlDataAccess db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        public async Task AddMessage(string senderEmail, string receiverEmail, string message, long timestamp)
        {
            var senderId = await _userRepository.GetUserId(senderEmail);
            var receiverId = await _userRepository.GetUserId(receiverEmail);

            var sql = "INSERT INTO messages (from_user, to_user, message_text, sent_timestamp) VALUES (@FromUser, @ToUser, @MessageText, @SentTimestamp)";

            var parameters = new Dictionary<string, object> {
                { "@FromUser", senderId.Value },
                { "@ToUser", receiverId.Value },
                { "@MessageText", message },
                { "@SentTimestamp", timestamp }
            };
            await _db.SaveData(sql, parameters);
        }

        public async Task<List<Message>> GetUsersMessages(int userId, int conversationPartnerId)
        {
            var sql = "SELECT * FROM messages WHERE (from_user = @UserId and to_user = @ConversationPartnerId) or (to_user = @UserId and from_user = @ConversationPartnerId)";

            var parameters = new Dictionary<string, object> {
                { "@UserId", userId },
                { "@ConversationPartnerId", conversationPartnerId }
            };

            var messages = await _db.LoadData<Message>(sql, parameters);

            return messages;
        }

        public async Task<List<int>> GetUsersMessagePartners(int userId)
        {
            var sql = "SELECT distinct(to_user) FROM messages where from_user = @UserId";
            var parameters = new Dictionary<string, object> {
                { "@UserId", userId }
            };
            var messagedPartners = await _db.LoadData<Message>(sql, parameters);

            sql = "SELECT distinct(from_user) FROM messages where to_user = @UserId";
            parameters = new Dictionary<string, object> {
                { "@UserId", userId }
            };
            var partnersWithMessageToUser = await _db.LoadData<Message>(sql, parameters);

            var partners = messagedPartners.Select(x => x.ToUser).ToList();

            foreach(var partner in partnersWithMessageToUser)
            {
                if (!partners.Contains(partner.FromUser))
                {
                    partners.Add(partner.FromUser);
                }
            }

            return partners;
        }
    }
}
