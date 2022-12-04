using MvcMovie.Models;

namespace MvcMovie.Services;

public class MemoryMovieService : IMovieService
{
    private readonly Movies _db = new Movies();

    private int id_increment;

    public async Task<Movies?> GetCollectionAsync() => throw new NotImplementedException();

    public void Add(Movie item)
    {
        ++id_increment;

        item.Id = id_increment;

        _db.Items.Add(item);
    }

    public bool CheckIfExists(int id)
    {
        foreach (var item in _db.Items )
        {
            if (item.Id == id)

                return true;
        }
        return false;
    }

    
    public void Delete(Movie item)
    {
        _db.Items.RemoveAt(item.Id - 1);
    }

    public Movie? GetById(int? id)
    {
        return _db.Items.FirstOrDefault(x => x.Id == id);
    }

    public Movies? GetCollection() => _db;

    public void Update(Movie item)
    {
        _db.Items[item.Id - 1].Title = item.Title;

        _db.Items[item.Id - 1].Description = item.Description;

        _db.Items[item.Id - 1].MovieTypeId = item.MovieTypeId;
    }

  
}