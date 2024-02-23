﻿// <auto-generated />
using System;
using Facebook.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Facebook.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240223144017_mysqlinitial5")]
    partial class mysqlinitial5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Facebook.API.Models.Like", b =>
                {
                    b.Property<int>("UserIsLikedId");

                    b.Property<int>("UserLikesId");

                    b.HasKey("UserIsLikedId", "UserLikesId");

                    b.HasIndex("UserLikesId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Facebook.API.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DateRead");

                    b.Property<DateTime>("DateSent");

                    b.Property<bool>("IsRead");

                    b.Property<bool>("RecipientDelete");

                    b.Property<int>("RecipientId");

                    b.Property<bool>("SenderDelete");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Facebook.API.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<string>("Url");

                    b.Property<int>("UserID");

                    b.Property<string>("public_id");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Facebook.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Car");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Description");

                    b.Property<string>("Gender");

                    b.Property<string>("Hobby");

                    b.Property<DateTime>("LastActive");

                    b.Property<string>("Motto");

                    b.Property<string>("Movies");

                    b.Property<string>("Music");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Personality");

                    b.Property<string>("Sport");

                    b.Property<string>("Username");

                    b.Property<string>("Work");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Facebook.API.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("Facebook.API.Models.Like", b =>
                {
                    b.HasOne("Facebook.API.Models.User", "UserIsLiked")
                        .WithMany("UserLikes")
                        .HasForeignKey("UserIsLikedId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Facebook.API.Models.User", "UserLikes")
                        .WithMany("UserIsLiked")
                        .HasForeignKey("UserLikesId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Facebook.API.Models.Message", b =>
                {
                    b.HasOne("Facebook.API.Models.User", "Recipient")
                        .WithMany("MessagesRecived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Facebook.API.Models.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Facebook.API.Models.Photo", b =>
                {
                    b.HasOne("Facebook.API.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
