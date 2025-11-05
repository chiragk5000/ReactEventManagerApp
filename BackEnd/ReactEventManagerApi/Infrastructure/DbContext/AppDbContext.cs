using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Infrastructure.DbContext
{
    public class AppDbContext : IdentityDbContext<User>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
     : base(options)
        {
            Activities = Set<Activity>();
            ActivityAttendees = Set<ActivityAttendee>();
            Photos = Set<Photo>();
        }

        public   DbSet<Activity> Activities { get; set; }
        public  DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        public  DbSet<Photo> Photos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.ActivityId, a.UserId }));

            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.User)
                .WithMany(x => x.Activites)
                .HasForeignKey(x => x.UserId);


            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.Activity)
                .WithMany(x => x.Attendees)
                .HasForeignKey(x => x.ActivityId);
        }
    }
}
