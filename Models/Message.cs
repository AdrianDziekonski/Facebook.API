using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }

        public User Sender { get; set; }

        public int RecipientId { get; set; }

        public User Recipient { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }  //? bo bedzie się pojawiać dopiero po odczytaniu wiec moze byc null

        public DateTime DateSent { get; set; }

        public bool SenderDelete { get; set; }

        public bool RecipientDelete { get; set; }

    }
}