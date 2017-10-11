using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class Message
    {
        public long Id { get; set; }
        //[Required]
        public string ToUser { get; set; }
        public string FromUser { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
