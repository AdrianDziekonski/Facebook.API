using System;
//using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Facebook.API.Data;
using Facebook.API.Dtos;
using Facebook.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Facebook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository repository, IConfiguration config)
        {
            this.config = config;
            this.repository = repository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto) //kontroler korzysta z DTO
        {

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); //po to zeby login zawsze był malymi literami

            if (await repository.UserExists(userForRegisterDto.Username))
                return BadRequest("Użytkownik o podanej nazwie już istnieje!");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await repository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto) //rutek zaleca from body przed User...
        {

            var userFromRepo = await repository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();
               

            //token logowania +korzystania z neigo jest taki ze server nie musi odpytywać bazy 

            //create Token      token w appSetings.json 
            //claims poswiadczenia
            var claims = new[]
                             {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
               };

                //było utf8 zamaist ascii
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
            

            //creds podpisanie poswiadczenia
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //token descriptor odczytuje token  Subject czego dotyczy, expires czas zycia tokenu, signingCredentials podpisanie/ potwierdzenie tokenu
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });


        }
    }
}

