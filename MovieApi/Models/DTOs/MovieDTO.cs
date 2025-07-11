using MovieApi.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public record MovieDTO(int id, string title, int year, string genre, int duration);
         
}
