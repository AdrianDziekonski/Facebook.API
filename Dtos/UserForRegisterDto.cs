using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//DTO ma za zadanie transferować dane pomiędzy systemami

namespace Facebook.API.Dtos
{
    
    public class UserForRegisterDto
    {
        [Required(ErrorMessage="Nazwa użytkownika jest wymagana!")] //pole wymagane
        public string Username { get; set; }
        [Required(ErrorMessage="Hasło jest wymagane!")]
        [StringLength(20,MinimumLength=4,ErrorMessage="Hasło musi zawierać od 4 do 18 znaków")] //ograniczenie znaków
        public string Password { get; set; }

        [Required]
        public string Gender {get;set;}
        
        [Required]
         public DateTime DateOfBirth {get;set;}

         [Required]
          public string City {get;set;}

          [Required]
           public string Country {get;set;}
            public DateTime Created {get;set;}
             public DateTime LastActive {get;set;}

             public UserForRegisterDto()
             {
                 Created= DateTime.Now;
                 LastActive= DateTime.Now;
             }
    }
}