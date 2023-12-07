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
        [StringLength(20,MinimumLength=6,ErrorMessage="Hasło musi zawierać od 6 do 18 znaków")] //ograniczenie znaków
        public string Password { get; set; }
    }
}