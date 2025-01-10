﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UrlShortener.Data;

#nullable disable

namespace UrlShortener.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250108135206_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UrlShortener.Models.ShortUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OriginalUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortenedUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ShortUrls");
                });

            modelBuilder.Entity("UrlShortener.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("74b58b42-e956-48c9-946c-73e9ec843be7"),
                            PasswordHash = "$2a$11$1Yj/sHKepI6GpHS8AhCJte7cwwbuyqjoFtd5UoFdMAUlGscMHgBsS",
                            Role = "Admin",
                            Username = "admin"
                        },
                        new
                        {
                            Id = new Guid("1280ac72-80c8-42d4-b4dd-083791ebc9a1"),
                            PasswordHash = "$2a$11$Z6UQielJq76KhCXju2YZLObHli4V4cVCnlu/b25C2hg9/L6bdceli",
                            Role = "User",
                            Username = "user"
                        });
                });

            modelBuilder.Entity("UrlShortener.Models.ShortUrl", b =>
                {
                    b.HasOne("UrlShortener.Models.User", "User")
                        .WithMany("ShortUrls")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UrlShortener.Models.User", b =>
                {
                    b.Navigation("ShortUrls");
                });
#pragma warning restore 612, 618
        }
    }
}
