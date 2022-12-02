using System.Data.SQLite;
using MvcMovie.Models;

namespace MvcMovie.Services;

public class DbMovieTypeService : IMovieTypeService
{
    private MovieTypes db = new MovieTypes();

    public SQLiteConnection conn = new SQLiteConnection(
        "Data Source= C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\App_Data\\movie.db; Version = 3; New = True; Compress = True; ");

    public SQLiteConnection CreateConnection()
    {
        // Open the connection:
        try
        {
            conn.Open();
        }
        catch (Exception ex)
        {
        }

        return conn;
    }

    public void Add(MovieType item)
    {
        SQLiteDataReader sqlite_datareader;

        SQLiteCommand sqlite_cmd;

        sqlite_cmd = conn.CreateCommand();

        sqlite_cmd.CommandText = "INSERT INTO MovieType(name) VALUES (" + "'" + item.Name + "'" + ") ;";


        sqlite_cmd.ExecuteReader();
    }


    public void ClearDatabase()
    {
        db.Items.Clear();
    }

    public void ReadData()
    {
        CreateConnection();

        SQLiteDataReader sqlite_datareader;

        SQLiteCommand sqlite_cmd;

        sqlite_cmd = conn.CreateCommand();

        sqlite_cmd.CommandText = "SELECT * FROM MovieType";

        sqlite_datareader = sqlite_cmd.ExecuteReader();


        while (sqlite_datareader.Read())
            db.Items.Add(new MovieType(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

        sqlite_datareader.Close();

        //conn.Close();
    }


    public void Update(MovieType item)
    {
        SQLiteDataReader sqlite_datareader;

        SQLiteCommand sqlite_cmd;

        sqlite_cmd = conn.CreateCommand();

        sqlite_cmd.CommandText =
            "UPDATE MovieType SET name = " + "'" + item.Name + "'" + " WHERE id = " + item.Id + ";";

        sqlite_datareader = sqlite_cmd.ExecuteReader();


        while (sqlite_datareader.Read())
            db.Items.Add(new MovieType(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

        sqlite_datareader.Close();

        //conn.Close();
    }

    public MovieTypes? GetCollection()
    {
        ReadData();

        return db;
    }

    public bool CheckIfExists(int id)
    {
        CreateConnection();

        SQLiteDataReader sqlite_datareader;

        SQLiteCommand sqlite_cmd;

        sqlite_cmd = conn.CreateCommand();

        sqlite_cmd.CommandText = "SELECT id FROM MovieType WHERE id = " + id;

        sqlite_datareader = sqlite_cmd.ExecuteReader();

        if (sqlite_datareader.HasRows)
        {
            //conn.Close();

            sqlite_datareader.Close();

            return true;
        }

        //conn.Close();

        sqlite_datareader.Close();

        return false;
    }

    public MovieType? GetById(int? id)
    {
        if (id == null)

            id = 1;

        CreateConnection();

        SQLiteDataReader sqlite_datareader;

        SQLiteCommand sqlite_cmd;

        sqlite_cmd = conn.CreateCommand();

        sqlite_cmd.CommandText = "SELECT * FROM MovieType WHERE id = " + id;


        sqlite_datareader = sqlite_cmd.ExecuteReader();

        //TODO: Handle Not Found
        while (sqlite_datareader.Read())
            db.Items.Add(new MovieType(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

        sqlite_datareader.Close();

        //conn.Close();

        return db.Items.FirstOrDefault(x => x.Id == id);
    }

    public void CloseConnection()
    {
        conn.Close();
    }
}