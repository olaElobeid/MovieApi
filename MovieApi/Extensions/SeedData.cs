using Bogus;
using System.Globalization;
using MovieApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Migrations;
using Bogus.DataSets;
using AutoMapper;



namespace MovieApi.Extensions
{
    public static class SeedData
    {
        private static Faker faker = new Faker("en");
        public static async Task InitAsync(this IApplicationBuilder app)
        {
            using var serviceProvider = app.ApplicationServices.CreateScope();
            var context = serviceProvider.ServiceProvider.GetRequiredService<MovieApiContext>();

            if (await context.Movie.AnyAsync()) return;

            //var actors = GenerateActors(50); //Skapa actors
            //await context.AddRangeAsync(actors);

            //var movies = GenerateMovies(10); //Skicka med actors
            //await context.AddRangeAsync(movies);



            await context.SaveChangesAsync();
        }


        private static IEnumerable<Movie> GenerateMovies(this IApplicationBuilder app)
        {
            {
                var movies = new List<Movie>((IEnumerable<Movie>)app);

                for (int i = 0; i < 10; i++)
                {
                    var movie = new Movie
                    {
                        Title = faker.Lorem.Sentence(1),
                        Year = faker.Date.Past(20).Year,
                        Genre = faker.PickRandom(new[] { "Action", "Comedy", "Drama", "Horror", "Sci-Fi", "Romance" }),
                        Duration = faker.Random.Int(60, 180),
                        MovieDetails = new MovieDetail
                        {
                            Budget = faker.Finance.Amount(1000000, 200000000, 2),
                            Synopsis = faker.Lorem.Paragraph(),
                            Language = faker.PickRandom(new[] { "English", "Spanish", "French", "German", "Chinese" })
                        },
                        Reviews = new List<Review>() //GenerateReviews(faker.Random.Int(1, 10))
                    {
                        new Review
                        {
                          Comment = faker.Lorem.Sentence(),
                           Rating = faker.Random.Int(1, 10),
                            ReviewerName = faker.Name.FullName()

                        },
                          new Review
                        {
                          Comment = faker.Lorem.Sentence(),
                           Rating = faker.Random.Int(1, 10),
                            ReviewerName = faker.Name.FullName()

                        }
                    },

                        Actors = new List<Actor>()
                    {
                        new Actor
                       {

                          Name = faker.Name.FullName(),
                          BirthYear = faker.Date.Past(50).Year
                       }
                   }
                    };

                    movies.Add(movie);
                }

                return movies;
            }
        }


    }
}