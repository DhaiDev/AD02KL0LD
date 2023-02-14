using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helper;

namespace MoviesAPI.Controllers
{
    [Route("api/MovieTheaters")]
    [ApiController]
 
    public class MovieTheatersController : ControllerBase
    {
         private readonly ILogger<MovieTheatersController> logger;
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public MovieTheatersController(  ILogger<MovieTheatersController> logger, ApplicationDBContext context, IMapper mapper)
        {
             this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheatersDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.MovieTheaters.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var actor = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();

            return mapper.Map<List<MovieTheatersDTO>>(queryable); ;
        }
 


        [HttpGet("{Id:int}")]
        public async Task<ActionResult<MovieTheatersDTO>> Get(int Id)
        {
            var movietheater = await context.MovieTheaters.FirstOrDefaultAsync(x => x.Id == Id);
            if (movietheater == null)
            {
                return NotFound();
            }

            return mapper.Map<MovieTheatersDTO>(movietheater);
        }



        [HttpPost]
        public async Task<ActionResult> Post(MovieTheaterCreationDTO movieCreationDTO)
        {
            var movieTheater = mapper.Map<MovieTheater>(movieCreationDTO);
            context.Add(movieTheater); // marking movietheater object yang kena add
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, MovieTheaterCreationDTO movieCreationDTO)
        {
            var movietheater = await context.MovieTheaters.FirstOrDefaultAsync(x => x.Id == id);
            if (movietheater == null)
            {
                return NotFound();
            }
            movietheater = mapper.Map(movieCreationDTO, movietheater);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movieTheater = await context.MovieTheaters.FirstOrDefaultAsync(x => x.Id == id);
            if (movieTheater == null)
            {
                return NotFound();
            }
            context.Remove(movieTheater);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
