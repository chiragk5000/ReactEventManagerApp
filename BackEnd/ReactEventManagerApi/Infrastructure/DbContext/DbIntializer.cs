using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.DbContext
{
    public class DbIntializer()
    {
        public static async Task SeedData(AppDbContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User{DisplayName="Bob",UserName="bob@test.com",Email="bob@test.com"},
                    new User{DisplayName="Tom",UserName="Tom@test.com",Email="tom@test.com"},
                    new User{DisplayName="John",UserName="John@test.com",Email="john@test.com"},

                };
                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
            if (context.Activities.Any()) return;
            var activities = ActivitySeedData();
            context.Activities.AddRange(activities);

        }

        public static List<Activity> ActivitySeedData()
        {
            var activities = new List<Activity>
            {
                new ()
                {
                Title = "Music Concert",
                Date = DateTime.UtcNow.AddDays(5),
                Description = "Live music concert featuring popular bands.",
                Category = "Music",
                IsCancelled = false,
                City = "New York",
                Venue = "Madison Square Garden",
                Latitude = 40.7505,
                Longitutde = -73.9934
                },
                new ()
                {
                Title = "Tech Conference",
                Date = DateTime.UtcNow.AddMonths(1),
                Description = "Annual technology conference with keynote speakers.",
                Category = "Technology",
                IsCancelled = false,
                City = "San Francisco",
                Venue = "Moscone Center",
                Latitude = 37.7847,
                Longitutde = -122.4011
                },
                new ()
                {
                Title = "Marathon",
                Date = DateTime.UtcNow.AddDays(30),
                Description = "City-wide marathon for charity.",
                Category = "Sports",
                IsCancelled = false,
                City = "Boston",
                Venue = "Boston Commons",
                Latitude = 42.3554,
                Longitutde = -71.0656
                },
                new ()
                {
                Title = "Art Exhibition",
                Date = DateTime.UtcNow.AddDays(10),
                Description = "Showcasing modern and contemporary art.",
                Category = "Art",
                IsCancelled = false,
                City = "Paris",
                Venue = "Louvre Museum",
                Latitude = 48.8606,
                Longitutde = 2.3376
                },
                new ()
                {
                Title = "Food Festival",
                Date = DateTime.UtcNow.AddDays(15),
                Description = "A celebration of world cuisines and local food vendors.",
                Category = "Food",
                IsCancelled = false,
                City = "Los Angeles",
                Venue = "LA Convention Center",
                Latitude = 34.0407,
                Longitutde = -118.2690
                }
            };
            return activities;
        }
    }
}
