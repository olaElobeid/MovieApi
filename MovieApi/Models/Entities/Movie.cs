using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;

namespace MovieApi.Models.Entities
{
    public class Movie
    {
        /*Id, Title, Year, Genre, Duration
(Ni får gärna normalisera tabellen Movie genom att bryta ut genre till egen tabell redan nu
eller annars kommer det som extra senare men se det som extra just nu)
• Relationer: 
o 1:1 med MovieDetails
o 1:M med Review
o N:M med Actor via MovieActor*/
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
