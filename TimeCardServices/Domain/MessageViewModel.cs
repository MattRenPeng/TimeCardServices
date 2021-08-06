using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardServices.Domain
{
    public class MessageViewModel
    {
        public string Field { get; set; }
        public string Message { get; set; }


        public string MessageType = "Error"; 
    }
}
