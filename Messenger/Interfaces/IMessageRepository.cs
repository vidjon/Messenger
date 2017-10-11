using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messenger.Models;

namespace Messenger.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        IEnumerable<Message> GetUnreadMessagesForUser(string userName);
        IEnumerable<Message> GetMessagesForUserByOffset(string userName, int startIndex, int stopIndex);
        void DeleteMessage(long id);
        void DeleteMessage(Message message);
        IEnumerable<Message> GetMessagesByUserAndIds(long[] ids, string userName);
    }
}
