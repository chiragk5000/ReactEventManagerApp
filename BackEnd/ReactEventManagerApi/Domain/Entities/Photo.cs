using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Photo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Url { get; set; } 

        public required string PublicId { get; set; }

        // nav properties 
        public string UserId { get; set; }

        [JsonIgnoreAttribute]
        public User user { get; set; } = null!;


    }
}
