using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MovieLibrary.Models;
using MovieLibrary.Services;

namespace MovieLibrary.UI
{
    public class MenuManager
    {
        private readonly IFileService _fileService;
        private readonly IMovieService _movieService;
        private List<MovieCatalog> _catalogs;
        private bool _flag = true;
        public MenuManager(IFileService fileService, IMovieService movieService)
        {
            _fileService = fileService;
            _movieService = movieService;
            _catalogs = _fileService.LoadCatalogs();
        }

        public void Run()
        {
            
            while (_flag) 
            {
                ShowMainMenu();
            }
        }

        private int ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== movie liblary ===");
            Console.WriteLine("1. create catalog");
            Console.WriteLine("2. open catalog");
            Console.WriteLine("3. remove catalog");
            Console.WriteLine("4. exit");

            Console.WriteLine("enter choose");

            string choice = Console.ReadLine();

            switch (choice) 
            {
                case ("1"):
                    CreateCatalog();
                    break;
                case ("2"):
                    OpenCatalogMenu();
                    break;
                case ("3"):
                    DeleteCatalog();
                    break;
                case ("4"):
                    _flag = false;
                    break;
            }
            return 1;
        }

        private void CreateCatalog()
        {
            Console.Clear();
            Console.WriteLine("enter name");
            string name = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(name))
            {
                ShowError("name can't be empty");
                return;
            }

            if (_catalogs.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                ShowError("Catalog alredy be");
                return;
            }

            _catalogs.Add(new MovieCatalog(name));
            _fileService.SaveCatalogs(_catalogs);
            ShowMessage("Catalog created");
        }

        private void OpenCatalogMenu()
        {
            if (_catalogs.Count < 0) 
            {
                ShowError("Catolog Count low zero");
            }
             while(true)
            {
                Console.Clear();
                Console.WriteLine("=== Choise catalog ===");

                for (int i = 0; i < _catalogs.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_catalogs[i].Name}:{_catalogs[i].Movies.Count}");
                }
                Console.WriteLine("0. Back");

                Console.WriteLine("choise catalog");
                if (!int.TryParse(Console.ReadLine(), out int index) || index == 0) return;

                if (index > 0 && index <= _catalogs.Count) 
                    ManageCatalog(_catalogs[index-1]);




            }
        }

        private void ManageCatalog(MovieCatalog catalog)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"=== Catalog: {catalog.Name} ===");
                Console.WriteLine($"1. add movie");
                Console.WriteLine($"2. pocazate all movie");
                Console.WriteLine($"3. find movie");
                Console.WriteLine($"4. delete movie");
                Console.WriteLine($"5. export movie file ");
                Console.WriteLine($"6. back ");

                string choise = Console.ReadLine();

                switch (choise)
                {
                    case ("1"):AddMovieToCatalog(catalog); break;
                    case ("2"):ShowAllMovies(catalog);break;
                    case ("3"):FindMovieInCatalog(catalog);break;
                    case ("4"):RemoveMovieFromCatalog (catalog);break;
                    case("5"):ExportMovieFromCatalog(catalog);break;
                    case("6"):_fileService.SaveCatalogs(_catalogs);return;
                }
            }
        }

        private void AddMovieToCatalog(MovieCatalog catalog)
        {
            Console.Clear();
            Console.WriteLine("=== add movie ===");

            Console.WriteLine("name movie");
            string title = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(title))
            {
                ShowError("name can't be empty");
                return;
            }

            Console.WriteLine("Year exicute");

            if (!int.TryParse(Console.ReadLine(), out int year)
                || year < 1888 || year > DateTime.Now.Year + 5)
            {
                ShowError("uncorrect");
                return;
            }

            Console.Write("genere cherez zapiatuiu");
            string genersInput = Console.ReadLine().Trim();
            var qenres = string.IsNullOrEmpty(genersInput)? new List<string>(): genersInput
                .Split(',')
                .Select(g => g.Trim())
                .Where(g => !string.IsNullOrEmpty(g))
                .ToList();
            var movie = new Movie(title, year, qenres);
            _movieService.AddMovie(catalog, movie);

            ShowMessage("movie add correct ");


        }

        private void ShowAllMovies(MovieCatalog catalog)
        {
            Console.Clear();
            Console.WriteLine($" === All movie In catalog ===");

            if (catalog.Movies.Count == 0)
            {
                Console.WriteLine("Catalog empty");
            }
            else 
            {
                for (int i = 0; i < catalog.Movies.Count; i++) 
                {
                    Console.WriteLine($"{i + 1} {catalog.Movies[i]}");
                }
            }
            WaitForContinue();
        }

        private void FindMovieInCatalog(MovieCatalog catalog)
        {
            Console.Clear();
            Console.WriteLine("Find movie");
            Console.WriteLine("enter title or genere for search");
            string query = Console.ReadLine().ToLower().Trim();

            if (string.IsNullOrEmpty(query))
            {
                ShowError("Search can't by empty");
                return;
            }

            var result = _movieService.FindMovies(catalog,query);

            foreach (var movie in result) 
            {
                Console.WriteLine($"- {movie}");
            }
            Console.WriteLine();
            WaitForContinue();

        }

        private void RemoveMovieFromCatalog(MovieCatalog catalog)
        {
            Console.Clear ();
            Console.WriteLine("remove movie");

            if(catalog.Movies.Count == 0)
            {
                ShowError("Catalog empty");
                return;
            }

            Console.WriteLine("movie list");
            for (int i = 0; i < catalog.Movies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {catalog.Movies[i]}");
            }
            Console.WriteLine("enter  number movie for remove (0 for escape)");
            if (!int.TryParse(Console.ReadLine(), out int index) || index == 0)
                return;

            if (index > 0 && index < catalog.Movies.Count)
            {
                _movieService.RemoveMovie(catalog, catalog.Movies[index - 1]);
                ShowMessage("movie remove");
            }
            else 
            {
                ShowError("uncorrect number");
            }

            

        }
        private void ExportMovieFromCatalog(MovieCatalog catalog) 
        {
            Console.Clear ();
            Console.WriteLine("export movie in file");

            if (catalog.Movies.Count == 0)
            {
                ShowError("catalog empty");
                return;
            }
            Console.WriteLine("list movie");
            for(int i = 0;i < catalog.Movies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {catalog.Movies[i]}");

            }
            Console.WriteLine("\n enter title movi for export");
            if (!int.TryParse(Console.ReadLine(), out int index) || index == 0) return;

            if (index > 0 && index <= catalog.Movies.Count)
            {
                var movie = catalog.Movies[index - 1];
                string fileName = $"movie_{movie.Title.Replace("", ", ")}.txt";

                try
                {
                    _fileService.ExportMovie(movie, fileName);
                    ShowMessage($"movie export in file");
                }
                catch (Exception ex)
                {
                    ShowError($"errore  for export {ex.Message}");

                }
            }
            else 
            {
                ShowError("uncorrect number");
            }

        }

        private void DeleteCatalog() 
        {
            if (_catalogs.Count == 0) 
            {
                ShowError("no dostupo");
                return; 
            }

            Console.Clear( );
            Console.WriteLine("=== delete Catalog ===");

            for (int i = 0; i < _catalogs.Count; i++)
            {
                Console.WriteLine($"{i+1} {_catalogs[i].Name} ");
            }

            Console.WriteLine("enter number");
            if (!int.TryParse(Console.ReadLine(), out int index) || index == 0)
            {
                return;
            }

            if (index > 0 && index <= _catalogs.Count) 
            {
                _catalogs.RemoveAt(index);
                _fileService.SaveCatalogs(_catalogs);
                ShowMessage("catalog is delete");
            }
            else
            {
                ShowError("uncorrect number");
            }

            
        }
        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
            WaitForContinue();
        }
        private void ShowError(string message) 
        {
            Console.WriteLine(message);
            WaitForContinue();
        }

        private void WaitForContinue()
        {
            Console.WriteLine("press any key for continue");
            Console.ReadKey();
        }
    }
}