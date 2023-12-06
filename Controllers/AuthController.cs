using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Data;
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
        public async Task<IActionResult> Register (string username,string password)
        {
            username=username.ToLower(); //po to zeby login zawsze był malymi literami

            if(await Repository.UserExists(username))
            return BadRequest("Użytkownik o podanej nazwie już istnieje!");

            var userToCreate=new User
            {
                Username=username
            };

            var createdUser= await Repository.Register(userToCreate,password);

            return StatusCode(201);
        }
    }
}