using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Helpers;
using Facebook.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.API.Data
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
           _context = context;
        }

   
        public async Task<User> GetUser(int id)
        {
            var user= await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(u=>u.Id==id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users= _context.Users.Include(p=>p.Photos).OrderBy(u=>u.Username).AsQueryable();

            //filtr nie wyswietlanie zalogowanego uzytkownika
             users=users.Where(u=>u.Id !=userParams.UserId);

             //filtorwanie po wieku
             if(userParams.MinAge !=15 || userParams.MaxAge!=100)
             {
             var minDate= DateTime.Today.AddYears(-userParams.MaxAge -1);
             var maxDate= DateTime.Today.AddYears(-userParams.MinAge);
             users= users.Where(u=> u.DateOfBirth >= minDate && u.DateOfBirth <=maxDate );
             }

             //sortowanie
             if(!string.IsNullOrEmpty(userParams.OrderBy))
             {
                switch (userParams.OrderBy)
                {
                    case "Userame":
                    users=users.OrderBy(u=> u.Username);
                    break;
                    default:
                    users= users.OrderByDescending(u=> u.Username);
                    break;
                }
             }

            return await PagedList<User>.CreateListAsync(users, userParams.PageNumber, userParams.PageSize);
        }

             public async Task<Photo> GetPhoto(int id)
        {
           var photo=await _context.Photos.FirstOrDefaultAsync(p=> p.Id==id);
           return photo;
        }

        public async Task<Photo> GetMainPhotoForUser (int userId) //zwraca zdjecie główne
        {
            return await _context.Photos.Where(u=>u.UserID==userId).FirstOrDefaultAsync(p=>p.IsMain);
        }
    }
}