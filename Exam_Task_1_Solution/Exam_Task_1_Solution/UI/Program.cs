using MovieLibrary.Services;
using MovieLibrary.UI;

class Program
{
    static int Main(string[] args)
    {
        IFileService fileServise = new FileService();
        IMovieService movieServise = new MovieService();
        MenuManager menager = new MenuManager(fileServise, movieServise);

        return menager.Run();
    }
}