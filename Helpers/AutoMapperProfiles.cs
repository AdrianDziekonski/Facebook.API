using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Facebook.API.Dtos;
using Facebook.API.Models;

namespace Facebook.API.Helpers
{
    public class AutoMapperProfiles : Profile //domyślny profil chyba
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserForListDto>()    
                .ForMember(dest=>dest.PhotoUrl, opt=>{
                    opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url); //pobieranie url photo
                })
                .ForMember(dest=>dest.Age, opt=>{
                    opt.ResolveUsing(src=>src.DateOfBirth.CalculateAge());
                })
                ;  
            CreateMap<User,UserForDetailsDto>()
            .ForMember(dest=>dest.PhotoUrl, opt=>{
                    opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url); //pobieranie url photo
                  })
                .ForMember(dest=>dest.Age, opt=>{
                    opt.ResolveUsing(src=>src.DateOfBirth.CalculateAge());
                })
                ;  
            CreateMap<Photo,PhotosForDetailsDto>();//<z czego,na co>

        }
    }
}