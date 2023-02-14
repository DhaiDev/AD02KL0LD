using NetTopologySuite.Geometries;

namespace MoviesAPI.DTOs
{
    public class MovieTheatersDTO
    {
        public int Id { get; set; }
 
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
