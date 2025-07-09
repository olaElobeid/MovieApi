using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;

namespace MovieApi.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int Duration { get; set; }
        // Navigation properties
        public MovieDetail MovieDetails { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
