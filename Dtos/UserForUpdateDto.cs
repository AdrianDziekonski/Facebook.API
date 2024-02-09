using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Dtos
{
    public class UserForUpdateDto
    {
         //informacje o użytkowniku
        public string Gender { get; set; }
        public int Age { get; set; }
    


        //zakładka info o uzytkowniku
        public string City { get; set; }
        public string Country { get; set; }
        public string Work { get; set; }

        public string Car { get; set; }

        //zakładka o mnie
        public string Motto { get; set; }
        public string Description { get; set; }
        public string Personality { get; set; }

        //Pasje, hobby
        public string Hobby { get; set; }
        public string Sport { get; set; }
        public string Movies { get; set; }
        public string Music { get; set; }
    }
}