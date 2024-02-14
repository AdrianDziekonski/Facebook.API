using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Helpers;
using Facebook.API.Models;

namespace Facebook.API.Data
{
    public interface IUserRepository : IGenericRepository
    {
        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(int id);

        Task<Photo> GetPhoto(int id);

        Task<Photo> GetMainPhotoForUser(int userId);
    }
}