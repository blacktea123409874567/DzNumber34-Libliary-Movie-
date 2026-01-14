using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MovieLibrary.Models;

namespace MovieLibrary.Services
{
    public class FileService : IFileService
    {
        private const string CatalogsFile = "catalogs.txt";

        public List<MovieCatalog> LoadCatalogs()
        {
            var catalogs = new List<MovieCatalog>();

            if (!File.Exists(CatalogsFile))
                return catalogs;

            try
            {
                string[] lines = File.ReadAllLines(CatalogsFile);
                MovieCatalog currentCatalog = null;

                foreach (string line in lines)
                {
                    if (line.StartsWith("CATALOG:"))
                    {
                        string catalogName = line.Substring(8).Trim();
                        currentCatalog = new MovieCatalog(catalogName);
                        catalogs.Add(currentCatalog);
                    }
                    else if (line.StartsWith("MOVIE:") && currentCatalog != null)
                    {
                        string[] parts = line.Substring(6).Split('|');
                        if (parts.Length >= 3)
                        {
                            var movie = new Movie(
                                parts[0],
                                int.Parse(parts[1]),
                                parts[2].Split(',').ToList()
                            );
                            currentCatalog.Movies.Add(movie);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return catalogs;
            }

            return catalogs;
        }


        public void SaveCatalogs(List<MovieCatalog> catalogs)
        {
            try
            {
                var lines = new List<string>();

                foreach (var catalog in catalogs)
                {
                    lines.Add($"CATALOG:{catalog.Name}");
                    foreach (var movie in catalog.Movies)
                    {
                        string genres = string.Join(",", movie.Genres);
                        lines.Add($"MOVIE:{movie.Title}|{movie.Year}|{genres}");
                    }
                }

                File.WriteAllLines(CatalogsFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void ExportMovie(Movie movie, string fileName)
        {
            try
            {
                string content = $"Название: {movie.Title}\n" +
                               $"Год выпуска: {movie.Year}\n" +
                               $"Жанры: {string.Join(", ", movie.Genres)}\n" +
                               $"Дата экспорта: {DateTime.Now}";

                File.WriteAllText(fileName, content);
            }
            catch (Exception)
            {
                throw new IOException($"Не удалось экспортировать фильм в файл: {fileName}");
            }
        }
    }
}
