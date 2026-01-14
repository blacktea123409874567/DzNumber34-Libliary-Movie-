using System.Collections.Generic;
using MovieLibrary.Models;

namespace MovieLibrary.Services
{
    public interface IFileService
    {
        List<MovieCatalog> LoadCatalogs();
        void SaveCatalogs(List<MovieCatalog> catalogs);
        void ExportMovie(Movie movie, string fileName);
    }
}