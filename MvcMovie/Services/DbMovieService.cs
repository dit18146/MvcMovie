using MvcMovie.Models;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MvcMovie.Services
{
    public class DbMovieService : IMovieService
    {
        private Movies db = new Movies();

        public SQLiteConnection conn = new SQLiteConnection("Data Source= C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\App_Data\\movie.db; Version = 3; New = True; Compress = True; ");
        

        public DbMovieService()
        {
          
            

        }
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

        public void ReadData()
        {

            CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT Movie.Id, Title, Description, Name FROM Movie, MovieType WHERE MovieType.Id = Movie.MovieTypeId";

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
               

                    db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1), sqlite_datareader.GetString(2), sqlite_datareader.GetString(3)));

                
            }

            sqlite_datareader.Close();

            //conn.Close();


        }


        public void Add(Movie item)
        {
            //CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO movie(title, description, MovieTypeId) VALUES (" + "'" + item.Title + "'" + "," + "' " + item.Description + "'" + ", " + item.MovieTypeId + ") ;";


            sqlite_cmd.ExecuteReader();

            //conn.Close();
        }



        public void Delete(Movie item)
        {
            //CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "DELETE FROM movie WHERE id = " + item.Id;


            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1), sqlite_datareader.GetString(2)));

            }

            sqlite_datareader.Close();

            //conn.Close();
        }

        public Movie? GetById(int? id)
        {
            if (id == null)

                id = 18;

            CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT * FROM Movie WHERE id = " +id;


            sqlite_datareader = sqlite_cmd.ExecuteReader();

            db.Items.Clear();   

            //TODO: Handle Not Found
            while (sqlite_datareader.Read())
            {
                db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1), sqlite_datareader.GetString(2), sqlite_datareader.GetInt32(3)));

            }

            sqlite_datareader.Close();

            return db.Items.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Movie item)
        {
            //CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "UPDATE movie SET title = " + "'" + item.Title + "'" + ", description = " + "'" + item.Description + "'" + "," + "movieTypeId = " + "'" + item.MovieTypeId + "'" + "  WHERE id = " + item.Id + ";";

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
                db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1), sqlite_datareader.GetString(2)));

            }

            sqlite_datareader.Close();

        }

        public Movies? GetCollection()
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

            sqlite_cmd.CommandText = "SELECT id FROM movie WHERE id = " + id;

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.HasRows)
            {
                sqlite_datareader.Close();

                return true;
            }

            else
            {
                sqlite_datareader.Close();

                return false;
            }

        }

        public void ClearDatabase()
        {
            db.Items.Clear();
        }

        public void CloseConnection()
        {
            conn.Close();
        }
    }
}
