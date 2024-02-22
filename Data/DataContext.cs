using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.API.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) {}  

        public  DbSet<Value> Values { get; set; }  
        ////To jest tabela do bazy danych
        
        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Like> Likes {get;set;}

        public DbSet<Message> Messages {get;set;}

//metoda do pokazania ze sa 2 glucze głowne 
 protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>().HasKey(k=> new {k.UserIsLikedId, k.UserLikesId});

            builder.Entity<Like>().HasOne(u=>u.UserIsLiked)
                                .WithMany(u=>u.UserLikes)
                                .HasForeignKey(u=> u.UserIsLikedId)
                                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>().HasOne(u=>u.UserLikes)
                                .WithMany(u=>u.UserIsLiked)
                                .HasForeignKey(u=> u.UserLikesId)
                                .OnDelete(DeleteBehavior.Restrict);  

           builder.Entity<Message>().HasOne(u=> u.Sender)
                                    .WithMany(m=>m.MessagesSent)
                                    .OnDelete(DeleteBehavior.Restrict);    

            builder.Entity<Message>().HasOne(u=> u.Recipient)
                                    .WithMany(m=>m.MessagesRecived)
                                    .OnDelete(DeleteBehavior.Restrict);                                                                  
        }
        
    }
}