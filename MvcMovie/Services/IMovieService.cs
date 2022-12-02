using MvcMovie.Models;

namespace MvcMovie.Services;

public interface IMovieService
{
    Movie? GetById(int? id);

    Movies? GetCollection();

    Task<Movies?> GetCollectionAsync();


    void Add(Movie item);

    void Update(Movie item);

    void Delete(Movie item);

    bool CheckIfExists(int id);


    //void Add_Category(Movie item);

    //void Update_Category(Movie item);

    

}