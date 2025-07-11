using AutoMapper;
using MovieApi;
using MovieApi.Models.DTOs;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            // Movie
            CreateMap<Movie, MovieCreateDto>().ReverseMap();
            // MovieDetail
            CreateMap<MovieDetail, MovieDetailDto>().ReverseMap();
            // Review
            CreateMap<Review, ReviewDto>().ReverseMap();
            // Actor
            CreateMap<Actor, ActorDto>().ReverseMap();

        }

    }
}
