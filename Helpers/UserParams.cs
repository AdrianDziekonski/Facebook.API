using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Helpers
{
    public class UserParams
    {
        public const int MaxPageSize = 48;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 24;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public int UserId { get; set; }
        public int Gender { get; set; }

        public int MinAge { get; set; }=15;
        public int MaxAge { get; set; }=100;

        public string OrderBy { get; set; }

        public bool UserLikes {get;set;}=false;

         public bool UserIsLiked {get;set;}=false;


    }
}