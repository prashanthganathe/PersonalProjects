using AngularJsAuthentication.API;
using AngularJSAuthentication.API.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MySqlInitializer());
        }
        //public AuthContext() : base("server=localhost;database=angularjsauth;password=;User Id=root;Persist Security Info=True;")
        //{
        //    Database.SetInitializer(new MySqlInitializer());
        //}

     

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

}