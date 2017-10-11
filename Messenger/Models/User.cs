using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
