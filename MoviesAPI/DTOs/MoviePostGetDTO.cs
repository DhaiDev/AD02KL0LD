namespace MoviesAPI.DTOs
{
    public class MoviePostGetDTO
    {
        public List<GenresDTO> Genres { get; set; }
        public List<MovieTheatersDTO> MovieTheaters { get; set; }    
    }
}
