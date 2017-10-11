using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messenger.Models;

namespace Messenger.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByUserName(string userName);
        IEnumerable<User> ListAll();
        void AddUser(User user);
        string SeedData();
        string ClearData();
        bool UserExists(string userName);
    }
}
