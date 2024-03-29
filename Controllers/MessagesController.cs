using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Facebook.API.Data;
using Facebook.API.Dtos;
using Facebook.API.Helpers;
using Facebook.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Helpers;

namespace Facebook.API.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]

    public class MessegesController : ControllerBase
    {
        public IUserRepository _repository { get; }
        public IMapper _mapper { get; }
        public MessegesController(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repository.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);

        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
             if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

              messageParams.UserId=userId;
              var messagesFromRepo= await _repository.GetMessagesForUser(messageParams);
              var messagesToReturn=_mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

              Response.AddPAgination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

              foreach (var message in messagesToReturn)
              {
                  message.MessageContainer=messageParams.MessageContainer;
              }
              
              return Ok(messagesToReturn);  
        }


        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessagesThread (int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo= await _repository.GetMessagesThread(userId,recipientId);
            var messageThread= _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);   

            return Ok(messageThread); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender= await _repository.GetUser(userId);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreationDto.SenderId = userId;

            var recipient = await _repository.GetUser(messageForCreationDto.RecipientId);

            if (recipient == null)
                return BadRequest("Nie można znaleźć użytkownika");

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repository.Add(message);

            

            if (await _repository.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new { id = message.Id }, messageToReturn);
            }
                

            throw new Exception("Utworznie wiadomości nie powiodło się podczas zapisu");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo=await _repository.GetMessage(id);

            if(messageFromRepo.SenderId==userId)
                messageFromRepo.SenderDelete=true;

            if(messageFromRepo.RecipientId==userId)
                messageFromRepo.RecipientDelete=true;

            if(messageFromRepo.SenderDelete && messageFromRepo.RecipientDelete)
            _repository.Delete(messageFromRepo);

            if(await _repository.SaveAll())
            return NoContent();

            throw new Exception("Błąd podczas usuwania wiadomości") ;           
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message=await _repository.GetMessage(id);

            if(message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

         if(await _repository.SaveAll())
            return NoContent();

            throw new Exception("Błąd") ; 

        }
    }
}