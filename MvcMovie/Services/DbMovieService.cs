using System.Data.SQLite;
using MvcMovie.Models;

namespace MvcMovie.Services;

public class DbMovieService : IMovieService
{
    private readonly SQLiteConnection _conn = new SQLiteConnection(
        "Data Source= C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\App_Data\\movie.db; Version = 3; New = True; Compress = True; ");


    public SQLiteConnection CreateConnection()
    {
        // Open the connection:
        try
        {
            _conn.Open();
        }
        catch (Exception ex)
        {
        }

        return _conn;
    }

    public async Task<Movies?> GetCollectionAsync() => throw new NotImplementedException();

    public void Add(Movie item)
    {
        var sqliteCmd = _conn.CreateCommand();

        sqliteCmd.CommandText = "INSERT INTO movie(title, description, MovieTypeId) VALUES (" + "'" + item.Title +
                                "'" + "," + "' " + item.Description + "'" + ", " + item.MovieTypeId + ") ;";

        sqliteCmd.ExecuteReader();
    }


    public void Delete(Movie item)
    {
        var sqliteCmd = _conn.CreateCommand();

        sqliteCmd.CommandText = "DELETE FROM movie WHERE id = " + item.Id;

        var sqliteDataReader = sqliteCmd.ExecuteReader();

        Movies _db = new Movies();

        while (sqliteDataReader.Read())
            _db.Items.Add(new Movie(sqliteDataReader.GetInt16(0), sqliteDataReader.GetString(1),
                sqliteDataReader.GetString(2)));

        sqliteDataReader.Close();
    }

    public Movie? GetById(int? id)
    {
        if (id == null)
            id = 18;

        CreateConnection();

        var sqliteCmd = _conn.CreateCommand();

        sqliteCmd.CommandText = "SELECT * FROM Movie WHERE id = " + id;


        var sqliteDataReader = sqliteCmd.ExecuteReader();

        Movies _db = new Movies();

        _db.Items.Clear();

        
        while (sqliteDataReader.Read())
            _db.Items.Add(new Movie(sqliteDataReader.GetInt16(0), sqliteDataReader.GetString(1),
                sqliteDataReader.GetString(2), sqliteDataReader.GetInt32(3)));

        sqliteDataReader.Close();

        return _db.Items.FirstOrDefault(x => x.Id == id);
    }

    public void Update(Movie item)
    {
        var sqliteCmd = _conn.CreateCommand();

        Movies _db = new Movies();

        sqliteCmd.CommandText = "UPDATE movie SET title = " + "'" + item.Title + "'" + ", description = " + "'" +
                                item.Description + "'" + "," + "movieTypeId = " + "'" + item.MovieTypeId + "'" +
                                "  WHERE id = " + item.Id + ";";

        var sqliteDataReader = sqliteCmd.ExecuteReader();

        while (sqliteDataReader.Read())
            _db.Items.Add(new Movie(sqliteDataReader.GetInt16(0), sqliteDataReader.GetString(1),
                sqliteDataReader.GetString(2)));

        sqliteDataReader.Close();
    }

    public Movies? GetCollection()
    {
        CreateConnection();

        Movies _db = new Movies();

        var sqliteCmd = _conn.CreateCommand();

        sqliteCmd.CommandText =
            "SELECT Movie.Id, Title, Description, Name, MovieType.id FROM Movie, MovieType WHERE MovieType.Id = Movie.MovieTypeId";

        var sqliteDataReader = sqliteCmd.ExecuteReader();


        while (sqliteDataReader.Read())
            _db.Items.Add(new Movie(sqliteDataReader.GetInt16(0), sqliteDataReader.GetString(1),
                sqliteDataReader.GetString(2), sqliteDataReader.GetString(3), sqliteDataReader.GetInt16(4)));

        sqliteDataReader.Close();


        return _db;
    }

    public bool CheckIfExists(int id)
    {
        CreateConnection();

        var sqliteCmd = _conn.CreateCommand();

        sqliteCmd.CommandText = "SELECT id FROM movie WHERE id = " + id;

        var sqliteDataReader = sqliteCmd.ExecuteReader();

        if (sqliteDataReader.HasRows)
        {
            sqliteDataReader.Close();

            return true;
        }

        sqliteDataReader.Close();

        return false;
    }

   

    public void CloseConnection()
    {
        _conn.Close();
    }
}