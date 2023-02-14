using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Helper
{
    public class AutoMapperProfiles: Profile 
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<V2ProfilerEntityDTO, v2Profiler>();
            CreateMap<V2ProfilerEntityDTO, v2Profiler>();

            CreateMap<GenresDTO, Genre>().ReverseMap();
            CreateMap<GenresCreationDTO, Genre>();

            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());


            CreateMap<MovieTheater, MovieTheatersDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
                .ForMember(X => X.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

            CreateMap<MovieTheaterCreationDTO, MovieTheater>()
                .ForMember(x => x.Location,x=> x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaterMovies, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));
        }


        private List<ActorsMovieDTO> MapMoviesActors(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<ActorsMovieDTO>();

            if (movie.MoviesGenres != null)
            {
                foreach (var moviesActor in movie.MoviesActors) 
                {
                    result.Add(new ActorsMovieDTO()
                    {
                        Id= moviesActor.ActorId,
                        Name=moviesActor.Actor.Name,
                        Character=moviesActor.Character,
                        Picture=moviesActor.Actor.Picture,
                        Order=moviesActor.Order
                    });
                }
            }

            return result;


        }


        private List<MovieTheatersDTO> MapMovieTheatersMovies(Movie movie, MovieDTO movieDTO) 
        {
            var result = new List<MovieTheatersDTO>();
            if (movie.MovieTheaterMovies != null) {
                foreach (var movieTheaterMovies in movie.MovieTheaterMovies)
                {
                    result.Add(new MovieTheatersDTO()
                    {
                        Id = movieTheaterMovies.MovieTheaterId,
                        Name = movieTheaterMovies.MovieTheater.Name,
                        Latitude = movieTheaterMovies.MovieTheater.Location.Y,
                        Longitude = movieTheaterMovies.MovieTheater.Location.X
                    });
                }
            }
            return result;
        }


        private List<GenresDTO> MapMoviesGenres(Movie movie, MovieDTO moviedto)
        {
            var result = new List<GenresDTO>();
            if (movie.MoviesGenres != null) {
                foreach (var genre in movie.MoviesGenres)
                {
                    result.Add(new GenresDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }
            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (movieCreationDTO.Actors == null) { return result; }

            foreach (var actor in movieCreationDTO.Actors) 
            {
                result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
            }

            return result;
        }

        private List<MovieTheaterMovies> MapMovieTheatersMovies(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieTheaterMovies>();
            if (movieCreationDTO.MovieTheaterIds == null) { return result; }

            foreach (var Id in movieCreationDTO.MovieTheaterIds) 
            {
                result.Add(new MovieTheaterMovies() { MovieTheaterId = Id });         
            }

            return result;
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if (movieCreationDTO.GenresIds == null) { return result; }

            foreach (var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }
    }
}
