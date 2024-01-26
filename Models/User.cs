using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        //informacje o użytkowniku
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastActive { get; set; }
      

        //zakładka info dodatkowe
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
        public string  Movies { get; set; }
        public string Music { get; set; }

        //Zdjęcia
        public ICollection<Photo> Photos { get; set; }
    }
}