using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Activity> Activities { get; }
        DbSet<ActivityAttendee> ActivityAttendees { get; }
        DbSet<User> Users { get; }

        DbSet<Photo> Photos { get; }

        DbSet<Comment> Comments { get; }

        DbSet<UserFollowing> UserFollowings { get; }



        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
