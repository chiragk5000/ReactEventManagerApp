using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            Comments = Set<Comment>();
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserFollowing> UserFollowings { get; set; }


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

            builder.Entity<UserFollowing>(x =>
            {
                x.HasKey(k => new { k.FollowerId, k.TargetUserId });

                x.HasOne(o => o.Follower)
                .WithMany(f => f.Followings)
                .HasForeignKey(o => o.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(o => o.TargetUser)
               .WithMany(f => f.Followers)
               .HasForeignKey(o => o.TargetUserId)
               .OnDelete(DeleteBehavior.Restrict);
            });

           

            DatetimeConverter(builder);


        }

        private void DatetimeConverter(ModelBuilder builder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                            );

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }

        }
    }
}

