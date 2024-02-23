using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Facebook.API.Dtos;
using Facebook.API.Models;

//mapowanie danych obiektow 

namespace Facebook.API.Helpers
{
    public class AutoMapperProfiles : Profile //domyślny profil chyba
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); //pobieranie url photo
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
                })
                ;
            CreateMap<User, UserForDetailsDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); //pobieranie url photo
            })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
                })
                ;
            CreateMap<Photo, PhotosForDetailsDto>();//<z czego,na co>
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));

        }
    }
}