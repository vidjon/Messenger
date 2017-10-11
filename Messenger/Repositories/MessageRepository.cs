using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messenger.Interfaces;
using Messenger.Models;

namespace Messenger.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddMessage(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
        }

        public void DeleteMessage(long id)
        {
            var message = new Message { Id = id };
            _dbContext.Messages.Attach(message);
            _dbContext.Messages.Remove(message);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Message> GetUnreadMessagesForUser(string userName)
        {
            var messages = _dbContext.Messages.Where(m => m.ToUser == userName && m.IsRead == false).ToList();

            foreach(Message message in messages)
            {
                message.IsRead = true;
            }

            _dbContext.SaveChanges();

            return messages.ToList();
        }

        public IEnumerable<Message> GetMessagesForUserByOffset(string userName, int startIndex, int stopIndex)
        {
            var messages = _dbContext.Messages.Where(m => m.ToUser == userName)
                .OrderBy(m => m.Date)
                .Skip(startIndex-1)
                .Take(stopIndex-startIndex+1)
                .ToList();

            foreach (Message message in messages)
            {
                message.IsRead = true;
            }

            _dbContext.SaveChanges();

            return messages.ToList();

        }

        public void DeleteMessage(Message message)
        {
            _dbContext.Messages.Remove(message);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Message> GetMessagesByUserAndIds(long[] ids, string userName)
        {
            return _dbContext.Messages.Where(m => ids.Contains(m.Id) && m.ToUser == userName).ToList();
        }
    }
}
