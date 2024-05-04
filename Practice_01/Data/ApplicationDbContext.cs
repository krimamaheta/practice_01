using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Numerics;
using Practice_01.Models;
using Practice_01.Authentication;

namespace Practice_01.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<VendorEvent> VendorEvents { get; set; }
        public DbSet<Models.Image> Images { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<ApiResponse> Responses { get; set; }
        public DbSet<ApplicationUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


             builder.Entity<ApiResponse>().HasNoKey();
        }

    }


}
