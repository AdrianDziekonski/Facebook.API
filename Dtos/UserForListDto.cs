using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Dtos
{
    public class UserForListDto
    {
         public int Id { get; set; }
        public string Username { get; set; }
 

        //informacje o użytkowniku
        public string Gender { get; set; }
        public int Age  { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastActive { get; set; }
      

        //zakładka info o uzytkowniku
          public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
    }
}