using MovieApi.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public record MovieDTO(int id, string title, int year, string genre, int duration);
    

    /*
     * MovieCreateDto (för POST) – med [Required], [Range] m.m. 
o MovieUpdateDto (för PUT) 
o MovieDto – sammanfattad vy 
o MovieDetailDto – innehåller: 
▪ Filmdata 
▪ MovieDetails 
▪ En lista av recensioner (List<ReviewDto>) 
▪ En lista av skådespelare (List<ActorDto>) 
o ReviewDto, ActorDto
namespace LtuUniversity.Models.Dtos;

public record StudentDto(int Id, string FullName, string Avatar, string AddressCity);

public record CourseDto
{
public string CourseTitle { get; init; } = string.Empty;
//[Range(0,5)]
public int Grade { get; init; }
}

public class StudentDetailsDto
{
public int Id { get; set; }
public required string FullName { get; set; }
public required string Avatar { get; set; }
public required string AddressCity { get; set; }
public IEnumerable<CourseDto> Enrollments { get; set; } = Enumerable.Empty<CourseDto>();
}


public record CreateStudentDto(string FirstName,[NotSameName] string LastName, string Avatar,[MaxNumber(10)] string AddressStreet, string AddressZipCode, string AddressCity);
public record UpdateStudentDto(string FirstName, string LastName, string Avatar, string AddressStreet, string AddressZipCode, string AddressCity);

     * */
       
}
