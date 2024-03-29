using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.API.Models;
using Newtonsoft.Json;

namespace Facebook.API.Data
{

    //metoda do odczytywania i wpisywania danych do utworzonych bazy

    //dane z generatoea json w pliku UserSeedData.json

    public class Seed
    {
        public DataContext _context { get; }
        public Seed(DataContext context)
        {
            _context = context;

        }

        public void SeedUsers()
        {
            if (!_context.Users.Any()) //dodaje tylko gdy w bazie nie ma uzytkowników
            {



                var userData = File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHashSalt("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    _context.Users.Add(user);

                }
                _context.SaveChanges();
            }
        }

        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

    }
}