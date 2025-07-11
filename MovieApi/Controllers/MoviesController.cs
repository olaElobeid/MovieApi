using AutoMapper;
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
using System.Threading.Tasks;
using Grpc.Core;


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
             return await _context.Movie.ToListAsync();
         }

        // GET: api/Movies/5
         [HttpGet("{id}")]
         public async Task<ActionResult<MovieCreateDto>> GetMovie(int id)
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

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

            // Mappa alla properties från DTO → entity
            mapper.Map(dto, movie);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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
    }
}
