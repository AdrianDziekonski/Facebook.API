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
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderBy(u => u.Username).AsQueryable();

            //filtr nie wyswietlanie zalogowanego uzytkownika
            users = users.Where(u => u.Id != userParams.UserId);

            //like
            if (userParams.UserLikes)
            {
                var userLikes = await GetUserLikes(userParams.UserId, userParams.UserLikes);
                users = users.Where(u => userLikes.Contains(u.Id));
            }
            if (userParams.UserIsLiked)
            {
                var userIsLiked = await GetUserLikes(userParams.UserId, userParams.UserLikes);
                users = users.Where(u => userIsLiked.Contains(u.Id));
            }

            //filtorwanie po wieku
            if (userParams.MinAge != 15 || userParams.MaxAge != 100)
            {
                var minDate = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDate = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDate && u.DateOfBirth <= maxDate);
            }

            //sortowanie
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "Userame":
                        users = users.OrderBy(u => u.Username);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.Username);
                        break;
                }
            }

            return await PagedList<User>.CreateListAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId) //zwraca zdjecie główne
        {
            return await _context.Photos.Where(u => u.UserID == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        //sprawdzenie czy uzytkonik dał juz like danej osobie
        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.UserLikesId == userId && u.UserIsLikedId == recipientId);
        }

        //pobierani lajkow do sprawdzenia wyzej
        private async Task<IEnumerable<int>> GetUserLikes(int id, bool userLikes)
        {
            var user = await _context.Users.Include(x => x.UserLikes)
                                            .Include(x => x.UserIsLiked)
                                            .FirstOrDefaultAsync(u => u.Id == id);

            if (userLikes)
            {
                return user.UserLikes.Where(u => u.UserIsLikedId == id).Select(i => i.UserLikesId);
            }
            else
            {
                return user.UserIsLiked.Where(u => u.UserLikesId == id).Select(i => i.UserIsLikedId);
            }
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messeges = _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                                            .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messeges = messeges.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDelete == false);
                    break;

                case "Outbox":
                    messeges = messeges.Where(u => u.SenderId == messageParams.UserId && u.SenderDelete == false);
                    break;
                default:
                    messeges = messeges.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false && u.RecipientDelete == false);
                    break;
            }

            messeges = messeges.OrderByDescending(d => d.DateSent);

            return await PagedList<Message>.CreateListAsync(messeges, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId)
        {
            var messeges = await _context.Messages
                              .Include(u => u.Sender).ThenInclude(p => p.Photos)
                              .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                              .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDelete == false
                              || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDelete == false).OrderByDescending(m => m.DateSent).ToListAsync();

            return messeges;
        }
    }
}