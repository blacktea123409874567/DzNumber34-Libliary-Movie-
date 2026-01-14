using System.Collections.Generic;

namespace MovieLibrary.Models
{
    public class MovieCatalog
    {
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }

        public MovieCatalog()
        {
            Movies = new List<Movie>();
        }

        public MovieCatalog(string name)
        {
            Name = name;
            Movies = new List<Movie>();
        }
    }
}