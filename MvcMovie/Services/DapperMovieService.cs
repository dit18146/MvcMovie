using System.Data;
using MvcMovie.Models;

namespace MvcMovie.Services;

public class DapperMovieService : IMovieService
{
    private readonly ISqlHelper _db;

    private readonly string _sqlConnString =
        "Data Source= C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\App_Data\\movie.db;";

    public DapperMovieService(ISqlHelper db) => _db = db;

    /// <inheritdoc />
    public async Task<Movies?> GetCollectionAsync()
    {
        var mitsos = await _db.QueryAsync<Movie>(_sqlConnString,
            "SELECT Movie.Id, Title, Description, Name FROM Movie, MovieType WHERE MovieType.Id = Movie.MovieTypeId",
            commandType: CommandType.Text).ConfigureAwait(false);

        var movies = new Movies
        {
            Items = mitsos.Value
        };
        return movies;
    }

    public void Add(Movie item)
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
        throw new NotImplementedException();
    }

    public Movie? GetById(int? id) => throw new NotImplementedException();

    public Movies? GetCollection() => throw new NotImplementedException();

    public void Update(Movie item)
    {
        throw new NotImplementedException();
    }
}