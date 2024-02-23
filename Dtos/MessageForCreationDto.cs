using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Dtos
{
    public class MessageForCreationDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }

        public DateTime DateSent { get; set; }

        public string Content { get; set; }

        public MessageForCreationDto()
        {
            DateSent=DateTime.Now;
        }
    }
}


///data transfer object 
//obiekt do transferu danych na prostsze