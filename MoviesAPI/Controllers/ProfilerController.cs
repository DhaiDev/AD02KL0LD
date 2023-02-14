 
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
    [Route("api/profiler")]
    [ApiController]

    public class ProfilerController : ControllerBase
    {
        private readonly ILogger<V2ProfilerEntityDTO> logger;
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ProfilerController(ILogger<V2ProfilerEntityDTO> logger, ApplicationDBContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }



        [HttpPost]
        public async Task<ActionResult> Post([FromBody] V2ProfilerEntityDTO v2ProfilerEntityDTO)
        {
            var profilerData = mapper.Map<v2Profiler>(v2ProfilerEntityDTO);
            context.Add(profilerData); 
            await context.SaveChangesAsync();
            return NoContent();
        }
 
 
    }
}
