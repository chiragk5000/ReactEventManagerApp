using Application.Prtofiles.DTOs;
using Domain.Entities;

namespace ReactEventManagerApi.DTOs
{
    public class ActivityDTO
    {
        public required string Id { get; set; } 

        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }

        public bool IsCancelled { get; set; }

        public required string HostId { get; set; }
        public required string HostDisplayName { get; set; }


        // location pprops

        public required string City { get; set; }

        public required string Venue { get; set; }

        public double Latitude { get; set; }
        public double Longitutde { get; set; }

        // nav properties 
        public ICollection<UserProfile> Attendees { get; set; } = [];
    }
}
