using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public class MovieCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [StringLength(15)]
        public string Genre { get; set; } = null!;

        [Required]
        [Range(1, 180)]
        public int Duration { get; set; }
    }
}
