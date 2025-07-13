using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Migrations;
using MovieApi.Models.DTOs;
using MovieApi.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieApiContext _context;
        private readonly IMapper mapper;

        public MoviesController(MovieApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            var query = _context.Movie
              .Include(m => m.Actors)
              .AsQueryable();

            var genre = Request.Query["genre"].ToString();
            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre.Contains(genre));

            var year = Request.Query["year"].ToString();
            if (int.TryParse(year, out int parsedYear))
                query = query.Where(m => m.Year == parsedYear);

            var actor = Request.Query["actor"].ToString();
            if (!string.IsNullOrEmpty(actor))
                query = query.Where(m => m.Actors.Any(a => a.Name.Contains(actor)));

            var movies = await query.ToListAsync();

            var dtos = movies.Select(m => mapper.Map<MovieDTO>(m)).ToList();

            return Ok(dtos);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieCreateDto>> GetMovie(int id, [FromQuery] bool withActors = false)
        {
            var query = _context.Movie
                       .Include(m => m.MovieDetails)
                       .Include(m => m.Reviews);

            if (withActors)
                query = (IIncludableQueryable<Movie, ICollection<Review>>)query.Include(m => m.Actors);

            var movie = await query.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var dto = mapper.Map<MovieDetailDto>(movie);
            return Ok(dto);
        }

        // GET: api/movies/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _context.Movie
               .Include(m => m.MovieDetails)
               .Include(m => m.Reviews)
               .Include(m => m.Actors)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var dto = mapper.Map<MovieDetailDto>(movie);
            return Ok(dto);
        }

        // Fix for CS1061: 'IEnumerable<object>' does not contain a definition for 'ToListAsync'
        // Explanation: The error occurs because 'ToListAsync' is an extension method for IQueryable<T>, not IEnumerable<T>. 
        // To fix this, ensure the query is of type IQueryable<T> before calling 'ToListAsync'.

        [HttpPost]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto, string genre)
        {
            var movie = mapper.Map<Movie>(dto);

            if (!string.IsNullOrEmpty(dto.Genre))
            {
                // Corrected the query to ensure it remains IQueryable<T> for 'ToListAsync' to work
                var genresQuery = _context.Movie
                    .Where(m => dto.Genre.Contains(m.Genre))
                    .Select(m => m.Genre); // Ensure the query is IQueryable<T>

                var genres = await genresQuery.ToListAsync(); // Call ToListAsync on IQueryable<T>

                movie.Genre = string.Join(", ", genres); // Combine genres into a single string
            }

            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            var movieDetailDto = mapper.Map<MovieDetailDto>(movie);

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDetailDto);
        }

        // Fix for CS1061: Replace the incorrect usage of '_context.MovieUpdateDto' with the correct DbSet for 'Actor' entities.

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var movie = await _context.Movie
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            // Map properties from DTO to entity
            mapper.Map(dto, movie);

            if (dto.ActorIds != null && dto.ActorIds.Any())
            {
                var actors = await _context.Actor
                    .Where(a => dto.ActorIds.Contains(a.Id))
                    .ToListAsync();

                movie.Actors = actors;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/actors
        [HttpGet("~/api/actors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
        {
            var actors = await _context.Actor.ToListAsync();
            var dtos = mapper.Map<List<ActorDto>>(actors);
            return Ok(dtos);
        }

        // GET: api/actors/{id}
        [HttpGet("~/api/actors/{id}")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
                return NotFound();

            var dto = mapper.Map<ActorDto>(actor);
            return Ok(dto);
        }

        // POST: api/actors
        [HttpPost("~/api/actors")]
        public async Task<ActionResult<ActorDto>> PostActor(ActorDto dto)
        {
            var actor = mapper.Map<Actor>(dto);
            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, mapper.Map<ActorDto>(actor));
        }




        // PUT: api/actors/{id}
        [HttpPut("~/api/actors/{id}")]
        public async Task<IActionResult> PutActor(int id, ActorDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
                return NotFound();

            mapper.Map(dto, actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/movies/{movieId}/actors/{actorId}
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movie
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            var actor = await _context.Actor.FindAsync(actorId);

            if (movie == null || actor == null)
                return NotFound();
            if (!movie.Actors.Contains(actor))
            {
                movie.Actors.Add(actor);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new movie", Description = "Creates a new movie with optional actors and reviews.", Tags = (["Movie"]))]
        //[ProducesResponseType(StatusCode = StatusCodes.Status201Created, Type = typeof(MovieCreateDto))]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = mapper.Map<Movie>(dto);

            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            var movieDetailDto = mapper.Map<MovieDetailDto>(movie);

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDetailDto);
        }

        // GET: api/movies/{movieId}/reviews
        [HttpGet("{movieId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews(int movieId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            var dtos = mapper.Map<List<ReviewDto>>(reviews);
            return Ok(dtos);
        }

        [HttpPost("{movieId}/reviews")]
        public async Task<ActionResult<ReviewDto>> PostReview(int movieId, ReviewDto dto)
        {
            var review = mapper.Map<Review>(dto);
            review.MovieId = movieId;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var createdDto = mapper.Map<ReviewDto>(review);
            return CreatedAtAction(nameof(GetReviews), new { movieId = movieId }, createdDto);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
        [HttpDelete("~/api/reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
