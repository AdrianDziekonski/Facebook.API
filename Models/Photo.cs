using System;

namespace Facebook.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
   
        public bool IsMain { get; set; }      //czy zdjecie jest głowne
        public string public_id {get; set;}  //potrzebne z chmuey cloudinary
        public User User { get; set; }  //odniesienie od uzytownika, po to żeby było usuwanie kaskadowe czyli po usunieciu uzytkownika usuwa zdjecia

        public int UserID {get;set;} //odniesienie do Id uzytkowanika
    }
}