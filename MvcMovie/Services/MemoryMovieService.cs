using MvcMovie.Models;

namespace MvcMovie.Services;

public class MemoryMovieService : IMovieService
{
    private readonly Movies _db;

    public MemoryMovieService() => _db = CreateDb(50);

    public void Add(Movie item)
    {
        _db.Items.Add(item);
    }

    public void Add_Category(Movie item)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfExists(int id) => throw new NotImplementedException();

    public void ClearDatabase()
    {
        throw new NotImplementedException();
    }

    public void CloseConnection()
    {
        throw new NotImplementedException();
    }

    public void Delete(Movie item)
    {
        _db.Items.Remove(item);
    }

    public Movie? GetById(int? id)
    {
        return _db.Items.FirstOrDefault(x => x.Id == id);
    }

    public Movies? GetCollection() => _db;

    public void Update(Movie item)
    {
        throw new NotImplementedException();
    }

    public void Update_Category(Movie item)
    {
        throw new NotImplementedException();
    }

    private Movies CreateDb(int size) => throw new NotImplementedException();
}