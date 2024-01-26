using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Facebook.API.Data;
using Facebook.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.API.Controllers
{

    [Authorize]
     [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {   

            var users=await _repo.GetUsers();

            var usersToReturn= _mapper.Map<IEnumerable<UserForListDto>>(users);
            
            return Ok(usersToReturn);
    



        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user= await _repo.GetUser(id);

            var userToReturn=_mapper.Map<UserForDetailsDto>(user);

            return Ok (userToReturn);
        }

    }
}