using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Dtos
{
    public class PhotoForReturnDto
    {
             public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
   
        public bool IsMain { get; set; }

        public string public_id { get; set; }
    
    }
}