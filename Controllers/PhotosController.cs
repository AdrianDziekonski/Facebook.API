using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Facebook.API.Data;
using Facebook.API.Dtos;
using Facebook.API.Helpers;
using Facebook.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Facebook.API.Controllers
{

    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]


    public class PhotosController : ControllerBase
    {
        public IUserRepository _repository { get; }
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _claudinary;
        public PhotosController(IUserRepository repository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repository = repository;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                 _cloudinaryConfig.Value.ApiKey,
                  _cloudinaryConfig.Value.ApiSecret
            );

            _claudinary = new Cloudinary(account);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))

                return Unauthorized();

            var userFromRepo = await _repository.GetUser(userId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if(file.Length>0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams= new ImageUploadParams()
                    {
                        File=new FileDescription(file.Name, stream),
                        Transformation=new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")  //transformation przekształca dodane zdjecie w podany sposob
                    };
                    uploadResult= _claudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url=uploadResult.Uri.ToString();
            photoForCreationDto.PublicID=uploadResult.PublicId;

            var photo= _mapper.Map<Photo>(photoForCreationDto);

            if(!userFromRepo.Photos.Any(p=> p.IsMain ))
            {
                    photo.IsMain=true;
            }

            userFromRepo.Photos.Add(photo);
            if (await _repository.SaveAll())
            {
                var photoToReturn=_mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id=photo.Id}, photoToReturn);
            }
            return BadRequest("Nie można dodać zdjęcia");
        }

        [HttpGet("{id}", Name= "GetPhoto")]
        public async Task<IActionResult> GetPhoto (int id)
        {
            var photoFromRepo= await _repository.GetPhoto(id);

            var photoForReturn= _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photoForReturn);
        }
    }
}