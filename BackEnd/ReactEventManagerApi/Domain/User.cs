
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User:IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }

        public string? ImageUrl { get; set; }

        // nav properties
        public ICollection<ActivityAttendee> Activites { get; set; } = [];

    }
}
