using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserFollowing
    {
        public required string FollowerId { get; set; }
        public User Follower { get; set; } = null!;

        public required string TargetUserId { get; set; }

        public User TargetUser { get; set; } = null!;



    }
}
