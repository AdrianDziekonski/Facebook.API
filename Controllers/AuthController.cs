using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Data;
using Facebook.API.Dtos;
using Facebook.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        public IAuthRepository Repository { get; }
        public AuthController(IAuthRepository repository)
        {
            this.Repository = repository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto) //kontroler korzysta z DTO
        {

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); //po to zeby login zawsze był malymi literami

            if (await Repository.UserExists(userForRegisterDto.Username))
                return BadRequest("Użytkownik o podanej nazwie już istnieje!");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await Repository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}