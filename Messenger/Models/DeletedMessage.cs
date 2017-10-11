using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class DeletedMessage
    {
        public long Id { get; set; }
        public string Status { get; set; }
    }

    public enum DeletedStatus
    {
        Deleted,
        NotFound
    }
}
