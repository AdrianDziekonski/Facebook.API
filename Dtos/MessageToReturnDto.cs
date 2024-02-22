using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }

        public string SenderUsername { get; set; }  //gdy było sendername, mapper nie wiadzaił o co chodzi dlatego treba SenderUsername bo Username jest w klasie User
        public string SenderPhotoUrl { get; set; }

        public int RecipientId { get; set; }
         public string RecipientPhotoUrl { get; set; }

        public string RecipientUsername { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }  //? bo bedzie się pojawiać dopiero po odczytaniu wiec moze byc null

        public DateTime DateSent { get; set; }

        public string MessageContainer { get; set; }

    }
}