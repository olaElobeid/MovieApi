namespace MovieApi.Models.DTOs
{
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Synopsis { get; set; } = null!;
        public string Language { get; set; } = null!;
        public int Budget { get; set; }

        public MovieDTO Movie { get; set; } = null!;
        public List<ReviewDto> Reviews { get; set; } = null!;
        public List<ActorDto> Actors { get; set; } = null!;
    }
}
