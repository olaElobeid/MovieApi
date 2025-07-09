using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data.Configurations;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public class MovieApiContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; } = default!;
        public MovieApiContext (DbContextOptions<MovieApiContext> options)
            : base(options)
        {
        }

   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MovieApiConfiguration());

        }
    }
}
