using Flexybook.Domain;
using Flexybook.Domain.Entities.Restaurant;

namespace Flexybook.Infrastructure.Seeders
{
    /// <summary>
    /// Seeds restaurant data into the database.
    /// </summary>
    public static class RestaurantSeeder
    {
        /// <summary>
        /// Seeds all restaurants into the provided database context.
        /// </summary>
        /// <param name="db">The restaurant database context.</param>
        public static void Seed(RestaurantContext db)
        {
            var aalborg = CreateAalborgRestaurant();
            var odense = CreateOdenseRestaurant();

            db.Restaurants.Add(aalborg);
            db.Restaurants.Add(odense);
        }

        /// <summary>
        /// Gets the ID of the Odense restaurant for use in user favorites.
        /// </summary>
        /// <returns>The GUID of the Odense restaurant.</returns>
        public static Guid GetOdenseRestaurantId()
        {
            return new Guid("47bb92e6-b2ad-4e33-a6bc-ee34dc30cd51");
        }

        private static Restaurant CreateAalborgRestaurant()
        {
            var id = Guid.NewGuid();
            
            var restaurant = new Restaurant
            {
                Id = id,
                Name = "Aalborg",
                Address = "Østerågade 27, 9000",
                City = "Aalborg",
                Telephone = "+45 11 22 33 44",
                Email = "aalborg@flexybox.com",
            };

            restaurant.Images = CreateAalborgImages(id, restaurant);
            restaurant.OpeningHours = CreateAalborgOpeningHours(id, restaurant);

            return restaurant;
        }

        private static Restaurant CreateOdenseRestaurant()
        {
            var id = GetOdenseRestaurantId();
            
            var restaurant = new Restaurant
            {
                Id = id,
                Name = "Odense",
                Address = "Nøglens Kvarter 181, 5220",
                City = "Odense",
                Telephone = "+45 52 82 82 21",
                Email = "odense@flexybox.com",
            };

            restaurant.Images = CreateOdenseImages(id, restaurant);
            restaurant.OpeningHours = CreateOdenseOpeningHours(id, restaurant);

            return restaurant;
        }

        private static List<Image> CreateAalborgImages(Guid restaurantId, Restaurant restaurant)
        {
            return new List<Image>
            {
                new Image
                {
                    Id = Guid.NewGuid(),
                    Base64Image = ImageConverter.ConvertToBase64("/images/restaurant-interior.jpg"),
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    Base64Image = ImageConverter.ConvertToBase64("/images/lee-campbell.jpg"),
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                }
            };
        }

        private static List<Image> CreateOdenseImages(Guid restaurantId, Restaurant restaurant)
        {
            return new List<Image>
            {
                new Image
                {
                    Id = Guid.NewGuid(),
                    Base64Image = ImageConverter.ConvertToBase64("/images/food.jpeg"),
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    Base64Image = ImageConverter.ConvertToBase64("/images/food2.jpeg"),
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    Base64Image = ImageConverter.ConvertToBase64("/images/food3.jpeg"),
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    Base64Image = ImageConverter.ConvertToBase64("/images/fancy-interior.jpg"),
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                }
            };
        }

        private static List<OpeningHour> CreateAalborgOpeningHours(Guid restaurantId, Restaurant restaurant)
        {
            var hours = new List<OpeningHour>();

            hours.AddRange(CreateRestaurantHours(restaurantId, restaurant, 7, 22));
            hours.AddRange(CreateTakeawayHours(restaurantId, restaurant));
            hours.AddRange(CreateBuffetHours(restaurantId, restaurant));
            hours.AddRange(CreateSpecialEventHours(restaurantId, restaurant));

            return hours;
        }

        private static List<OpeningHour> CreateOdenseOpeningHours(Guid restaurantId, Restaurant restaurant)
        {
            var hours = new List<OpeningHour>();

            // Odense has different hours: Mon-Wed 10-22, Thu-Sun 8-22
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Monday, 10, 22));
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Tuesday, 10, 22));
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Wednesday, 10, 22));
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Thursday, 8, 22));
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Friday, 8, 22));
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Saturday, 8, 22));
            hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, DayOfWeek.Sunday, 8, 22));
            hours.Add(CreateClosedHour(restaurantId, restaurant, OpeningHourType.Restaurant));

            hours.AddRange(CreateTakeawayHours(restaurantId, restaurant));
            hours.AddRange(CreateOdenseSpecialEventHours(restaurantId, restaurant));

            return hours;
        }

        private static List<OpeningHour> CreateRestaurantHours(Guid restaurantId, Restaurant restaurant, int openHour, int closeHour)
        {
            var hours = new List<OpeningHour>();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Restaurant, day, openHour, closeHour));
            }

            hours.Add(CreateClosedHour(restaurantId, restaurant, OpeningHourType.Restaurant));

            return hours;
        }

        private static List<OpeningHour> CreateTakeawayHours(Guid restaurantId, Restaurant restaurant)
        {
            var hours = new List<OpeningHour>();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                hours.Add(CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Takeaway, day, 12, 22));
            }

            hours.Add(CreateClosedHour(restaurantId, restaurant, OpeningHourType.Takeaway));

            return hours;
        }

        private static List<OpeningHour> CreateBuffetHours(Guid restaurantId, Restaurant restaurant)
        {
            return new List<OpeningHour>
            {
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Monday),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Tuesday),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Wednesday),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Thursday),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Friday, 16, 22),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Saturday, 16, 22),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.Buffet, DayOfWeek.Sunday, 16, 22),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.Buffet)
            };
        }

        private static List<OpeningHour> CreateSpecialEventHours(Guid restaurantId, Restaurant restaurant)
        {
            return new List<OpeningHour>
            {
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Monday, 14, 0),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Tuesday, 14, 0),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Wednesday, 14, 0),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Thursday, 14, 0),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Friday, 14, 2),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Saturday, 14, 2),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Sunday, 14, 2),
                new OpeningHour
                {
                    Id = Guid.NewGuid(),
                    Type = OpeningHourType.SpecialEventForGroups,
                    OpenTime = new TimeSpan(14, 0, 0),
                    CloseTime = new TimeSpan(2, 0, 0),
                    IsClosed = false,
                    RestaurantId = restaurantId,
                    Restaurant = restaurant
                }
            };
        }

        private static List<OpeningHour> CreateOdenseSpecialEventHours(Guid restaurantId, Restaurant restaurant)
        {
            return new List<OpeningHour>
            {
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Monday),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Tuesday),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Wednesday),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Thursday),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Friday, 14, 2),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Saturday, 14, 2),
                CreateOpeningHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups, DayOfWeek.Sunday, 14, 2),
                CreateClosedHour(restaurantId, restaurant, OpeningHourType.SpecialEventForGroups)
            };
        }

        private static OpeningHour CreateOpeningHour(
            Guid restaurantId,
            Restaurant restaurant,
            OpeningHourType type,
            DayOfWeek dayOfWeek,
            int openHour,
            int closeHour)
        {
            return new OpeningHour
            {
                Id = Guid.NewGuid(),
                Type = type,
                DayOfWeek = dayOfWeek,
                OpenTime = new TimeSpan(openHour, 0, 0),
                CloseTime = new TimeSpan(closeHour, 0, 0),
                IsClosed = false,
                RestaurantId = restaurantId,
                Restaurant = restaurant
            };
        }

        private static OpeningHour CreateClosedHour(
            Guid restaurantId,
            Restaurant restaurant,
            OpeningHourType type,
            DayOfWeek? dayOfWeek = null)
        {
            var hour = new OpeningHour
            {
                Id = Guid.NewGuid(),
                Type = type,
                IsClosed = true,
                RestaurantId = restaurantId,
                Restaurant = restaurant
            };

            if (dayOfWeek.HasValue)
            {
                hour.DayOfWeek = dayOfWeek.Value;
            }

            return hour;
        }
    }
}
