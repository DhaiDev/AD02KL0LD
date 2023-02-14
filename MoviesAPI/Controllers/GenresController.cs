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
    [Route("api/genres")]
    [ApiController]
 
    public class GenresController : ControllerBase
    {
         private readonly ILogger<GenresController> logger;
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public GenresController(  ILogger<GenresController> logger, ApplicationDBContext context, IMapper mapper)
        {
             this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenresDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Genres.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var genres =  await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();

            return mapper.Map<List<GenresDTO>>(genres); ;
        }

        [HttpGet("{Id:int}")]  
        public async Task<ActionResult<GenresDTO>> Get(int Id ) 
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            if (genre == null)
            {
                return NotFound();
            }

            return mapper.Map<GenresDTO>(genre);
        }



        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenresCreationDTO genresCreationDTO)
        {
            var genre = mapper.Map<Genre>(genresCreationDTO);
            context.Add(genre); // marking genre object yang kena add
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async  Task<ActionResult> Put(int id,[FromBody] GenresCreationDTO genresCreationDTO)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            genre = mapper.Map(genresCreationDTO, genre);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) 
        {
            var exists = await context.Genres.AnyAsync(x => x.Id == id);
            if (!exists) 
            {
                return NotFound();
            }
            context.Remove(new Genre() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
         }

    }
}
