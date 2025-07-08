namespace MovieApi.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; } // Assuming Rating is an integer between 1 and 5
        // Foreign key
        public int MovieId { get; set; }
        // Navigation property
        public Movie Movie { get; set; } = null!;
    }
}
