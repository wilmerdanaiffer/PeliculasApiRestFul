using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using PeliculasApiRestFul.Entidades;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Claims;

namespace PeliculasApiRestFul
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //private void SeedData(ModelBuilder modelBuilder)
        //{
        //    var rolAdminId = "aa5c881a-8bff-4cf3-9b53-1c638954ce98";
        //    var usuarioAdminId = "fa686ac7-2580-4262-81b2-29ffb20d4a9d";

        //    var rolAdmin = new IdentityRole()
        //    {
        //        Id = rolAdminId,
        //        Name = "Admin",
        //        NormalizedName = "Admin"
        //    };
        //    var passwordHasher = new PasswordHasher<IdentityUser>();
        //    var username = "wilmer994@gmail.com";
        //    var userAdmin = new IdentityUser()
        //    {
        //        Id = usuarioAdminId,
        //        UserName = username,
        //        NormalizedUserName = username,
        //        Email = username,
        //        NormalizedEmail = username,
        //        PasswordHash = passwordHasher.HashPassword(null, "P4$$l3cf1r3")
        //    };
        //    modelBuilder.Entity<IdentityUser>().HasData(userAdmin);
        //    modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);
        //    modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
        //        new IdentityUserClaim<string>()
        //        {
        //            Id = 1,
        //            ClaimType = ClaimTypes.Role,
        //            UserId = usuarioAdminId,
        //            ClaimValue = "Admin"
        //        });
        //}

        public DbSet<Genero> Generos { get; set; }
        //public DbSet<Actor> Actores { get; set; }
        //public DbSet<Pelicula> Peliculas { get; set; }
        //public DbSet<PeliculasActores> PeliculasActores { get; set; }
        //public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}
