using MvcMovie.Models;

namespace MvcMovie.Services
{
    public interface IMovieTypeService
    {
        MovieTypes? GetCollection();

        void Add(MovieType item);

        void Update(MovieType item);

        void ClearDatabase();

        bool CheckIfExists(int id);

        MovieType? GetById(int? id);

        void CloseConnection();


    }
}
