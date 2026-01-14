using System.Collections.Generic;
using MovieLibrary.Models;

namespace MovieLibrary.Services
{
    public interface IMovieService
    {
        void AddMovie(MovieCatalog catalog, Movie movie);
        void RemoveMovie(MovieCatalog catalog, Movie movie);
        List<Movie> FindMovies(MovieCatalog catalog, string query);
        bool MovieExists(MovieCatalog catalog, string title);
    }
}