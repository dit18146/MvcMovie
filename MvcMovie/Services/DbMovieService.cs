using MvcMovie.Models;
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

            sqlite_cmd.CommandText = "SELECT * FROM Movie";

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
               

                    db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

                
            }

            //conn.Close();


        }


        public void Add(Movie item)
        {
            //CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO movie(id, title) VALUES (" + item.Id + " , " + "'" + item.Title + "' ) ;";


            sqlite_datareader = sqlite_cmd.ExecuteReader();

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
                db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

            }

            //conn.Close();
        }

        public Movie? GetById(int? id)
        {
            if (id == null)

                id = 1;

            CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT * FROM Movie WHERE id = " +id;


            sqlite_datareader = sqlite_cmd.ExecuteReader();

            //TODO: Handle Not Found
            while (sqlite_datareader.Read())
            {
                db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

            }
            //conn.Close();

            return db.Items.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Movie item)
        {
            //CreateConnection();

            SQLiteDataReader sqlite_datareader;

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "UPDATE movie SET title = " + "'" + item.Title + "'" + " WHERE id = " + item.Id;

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
                db.Items.Add(new Movie(sqlite_datareader.GetInt16(0), sqlite_datareader.GetString(1)));

            }

            //conn.Close();

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
                //conn.Close();

                return true;
            }

               

            else
            {
                //conn.Close();

                return false;
            }
           

            

        }

        public void ClearDatabase()
        {
            db.Items.Clear();
        }
    }
}
