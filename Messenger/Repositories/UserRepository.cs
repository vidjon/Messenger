using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messenger.Interfaces;
using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }

        public string ClearData()
        {
            var users = _dbContext.Users.ToList();
            var messages = _dbContext.Messages.ToList();

            foreach (User user in users)
            {
                _dbContext.Users.Remove(user);
            }

            foreach (Message message in messages)
            {
                _dbContext.Messages.Remove(message);
            }

            _dbContext.SaveChanges();

            return "Data cleared";
        }

        public User GetUserByUserName(string userName)
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
        }

        public bool UserExists(string userName)
        {
            var user = _dbContext.Users.Where(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefault();

            return user == null ? false : true;
        }

        public IEnumerable<User> ListAll()
        {
            return _dbContext.Users.AsEnumerable();
        }

        public string SeedData()
        {
            if(_dbContext.Users.Count() == 0 && _dbContext.Messages.Count() == 0)
            {
                _dbContext.Users.Add(new User {UserName = "harper" });
                _dbContext.Users.Add(new User {UserName = "jordan" });
                _dbContext.Users.Add(new User {UserName = "pippen" });
                _dbContext.Users.Add(new User {UserName = "rodman" });
                _dbContext.Users.Add(new User {UserName = "longley" });
                _dbContext.Users.Add(new User { UserName = "jackson" });

                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 9, 12)});
                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 9, 13) });
                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 9, 14) });
                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 9, 15) });
                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 9, 16) });
                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 9, 17) });
                _dbContext.Messages.Add(new Message { ToUser = "harper", FromUser = "jordan", Content = "Pass the ball", Date = new DateTime(1995, 11, 12) });
                _dbContext.Messages.Add(new Message { ToUser = "jordan", FromUser = "harper", Content = "No way !", Date = new DateTime(1995, 11, 13) });
                _dbContext.Messages.Add(new Message { ToUser = "jordan", FromUser = "rodman", Content = "Get your shit together", Date = new DateTime(1995, 12, 14) });
                _dbContext.Messages.Add(new Message { ToUser = "pippen", FromUser = "jordan", Content = "You are nothing without me!", Date = new DateTime(1995, 12, 24) });
                _dbContext.Messages.Add(new Message { ToUser = "pippen", FromUser = "longley", Content = "Hey Mate !", Date = new DateTime(1996, 1, 3) });
                _dbContext.Messages.Add(new Message { ToUser = "rodman", FromUser = "jackson", Content = "Where the hell are you?", Date = new DateTime(1995, 10, 1) });
                _dbContext.Messages.Add(new Message { ToUser = "rodman", FromUser = "harper", Content = "Just rebound the ball, ok?", Date = new DateTime(1996, 2, 12) });
                _dbContext.Messages.Add(new Message { ToUser = "longley", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 11, 12) });
                _dbContext.Messages.Add(new Message { ToUser = "longley", FromUser = "jackson", Content = "No Practice Tonight", Date = new DateTime(1995, 11, 12) });
                _dbContext.Messages.Add(new Message { ToUser = "jackson", FromUser = "rodman", Content = "I'm hungover", Date = new DateTime(1995, 10, 2) });
                _dbContext.Messages.Add(new Message { ToUser = "jackson", FromUser = "jordan", Content = "We are going to three peat again!", Date = new DateTime(1996, 7, 1) });
                _dbContext.SaveChanges();

                return "Data seeded, users harper, jordan, pippen, rodman, longley and jackson created with messages. Psst, Harper seems to have a lot of messages";
            }
            else
            {
                return "Clear Data before seeding new data";
            }
        }
    }
}
