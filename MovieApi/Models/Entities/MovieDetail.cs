namespace MovieApi.Models.Entities
{
    public class MovieDetail
    {
        public int Id { get; set; }
        public string Synopsis { get; set; } = null!;
        public string Language { get; set; } = null!;
        public decimal Budget { get; set; }
        // Foreign key
        public int MovieId { get; set; }
        // Navigation property
        public Movie Movie { get; set; } = null!;
    }
}
