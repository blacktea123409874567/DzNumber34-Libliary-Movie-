using System;
using System.Collections.Generic;
using System.Linq;
using MovieLibrary.Models;

namespace MovieLibrary.Services
{
    public class MovieService : IMovieService
    {
        public void AddMovie(MovieCatalog catalog, Movie movie)
        {
            if (catalog == null || movie == null)
                throw new ArgumentNullException();

            catalog.Movies.Add(movie);
        }

        public void RemoveMovie(MovieCatalog catalog, Movie movie)
        {
            if (catalog == null || movie == null)
                throw new ArgumentNullException();

            catalog.Movies.Remove(movie);
        }

        public List<Movie> FindMovies(MovieCatalog catalog, string query)
        {
            if (string.IsNullOrEmpty(query) || catalog == null)
                return new List<Movie>();

            return catalog.Movies
                .Where(m => m.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                           m.Genres.Any(g => g.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public bool MovieExists(MovieCatalog catalog, string title)
        {
            return catalog?.Movies.Any(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase)) ?? false;
        }
    }
}