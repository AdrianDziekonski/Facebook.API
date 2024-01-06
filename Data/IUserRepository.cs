using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Models;

namespace Facebook.API.Data
{
    public interface IUserRepository : IGenericRepository
    {
        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(int id);
    }
}