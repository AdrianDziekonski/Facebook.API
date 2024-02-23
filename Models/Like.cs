//models to szablon jak ma wyglądać dany obiekt,  w klasie podajemy mu propy ktroe przyjmuje


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Models
{
    public class Like
    {
        public int UserLikesId { get; set; } //uzytkownik który wysyła lika
        public int UserIsLikedId { get; set; } //uzytkownik ktory dostaje like

        public User UserLikes { get; set; }
        public User UserIsLiked { get; set; }
    }
}