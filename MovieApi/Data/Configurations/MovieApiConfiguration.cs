using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using MovieApi.Models.Entities;

namespace MovieApi.Data.Configurations
{
    public class MovieApiConfiguration : IEntityTypeConfiguration<Movie>
    {

        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");

            builder.HasKey(m => m.Id);


        }
    }
      
    
    
}
