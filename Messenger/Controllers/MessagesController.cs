using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger.Interfaces;
using Messenger.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Messenger.Controllers
{
    [Route("api/user/{userName}/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public MessagesController(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets the messages for the current user. If the start and stop index are used (both need to be used) all messages are fetched ordered by time with start and stop index option.
        /// If no indexes are selected: all unread messages are shown for the user.
        /// </summary>
        /// <remarks>
        /// Sample requests:
        ///
        ///     Get /User/{userName}/Messages
        ///     
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Messages for the user.</returns>
        /// <response code="200">Returns the messages in accordance with the request.</response>
        [HttpGet(Name = "GetMessages")]
        public IEnumerable<Message> GetMessagesForUser(string userName, [FromQuery] int startIndex = 0, [FromQuery] int stopIndex = 0)
        {
            if (startIndex == 0 || stopIndex == 0)
            {
                return _messageRepository.GetUnreadMessagesForUser(userName);
            }
            else
            {
                return _messageRepository.GetMessagesForUserByOffset(userName, startIndex, stopIndex);
            }
        }

        /// <summary>
        /// Sends a message from the current user to a user specificed in the message.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User/{userName}/Messages
        ///     {
        ///        "toUser": "vidjon",
        ///        "content": "Hello world!"
        ///     }
        ///
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>A newly-created User and the route to the user.</returns>
        /// <response code="201">Returns the newly-created message and the location for the sender's messages.</response>
        /// <response code="400">If the toUser is null or empty or the recipient does not exist in database.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Message), 201)]
        public IActionResult SendMessage(string userName, [FromBody]Message message)
        {
            if (string.IsNullOrEmpty(message.ToUser))
            {
                return BadRequest();
            }
            else if (!_userRepository.UserExists(message.ToUser))
            {
                return BadRequest();
            }
            //check if user exist in database, if not

            message.FromUser = userName;
            message.Date = DateTime.UtcNow;
            message.IsRead = false;

            _messageRepository.AddMessage(message);
            
            //respond with messages sent

            return CreatedAtRoute("GetMessages",message);
        }

        /// <summary>
        /// Deletes specific messages for a user based on list of Ids.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /User/{userName}/Messages?ids=1
        ///
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Returns an array with the ids of the messages requested to delete and if they were deleted or not found on server.</returns>
        /// <response code="200">Array with status of each of the messages that was requested to delete.</response>
        [HttpDelete]
        [ProducesResponseType(typeof(DeletedMessage), 200)]
        public IEnumerable<DeletedMessage> Delete(string userName, [FromQuery] long[] ids)
        {
            var messagesInDb = _messageRepository.GetMessagesByUserAndIds(ids, userName);

            List<DeletedMessage> deletedMessages = new List<DeletedMessage>();
            Message tempMessage = null;

            foreach(long id in ids)
            {
                tempMessage = messagesInDb.Where(m => m.Id == id).FirstOrDefault();
                
                if(tempMessage != null)
                {
                    _messageRepository.DeleteMessage(tempMessage);
                    deletedMessages.Add(new DeletedMessage { Id = id, Status = DeletedStatus.Deleted.ToString() });
                    tempMessage = null;
                }
                else
                {
                    deletedMessages.Add(new DeletedMessage { Id = id, Status = DeletedStatus.NotFound.ToString() });
                }
            }

            return deletedMessages;
        }
    }
}
