using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Models
{
    public class Movie
    {
        public string Title { get; private set; }
        public int Year { get; private set; }
        public List<string> Genres { get; private set; }

        public Movie()
        {
            Genres = new List<string>();
        }

        public Movie(string title, int year, List<string> genres)
        {
            Title = title;
            Year = year;
            Genres = genres ?? new List<string>();
        }

        public override string ToString()
        {
            return $"{Title} ({Year}) - {string.Join(", ", Genres)}";
        }
    }
}
