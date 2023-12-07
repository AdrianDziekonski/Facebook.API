using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//DTO ma za zadanie transferować dane pomiędzy systemami

namespace Facebook.API.Dtos
{
    public class UserForRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}