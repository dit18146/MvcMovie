using MvcMovie.Models;
using System.Data;
using System.Data.SQLite;

namespace MvcMovie.Services
{
    public class DbFileService : IFileservice
    {
         private readonly SQLiteConnection _conn = new SQLiteConnection(
        "Data Source= C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\App_Data\\movie.db; Version = 3; New = True; Compress = True; ");
        
        private readonly string _sqlConnString =
        "Data Source= C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\App_Data\\movie.db;";

        
        private readonly ISqlHelper _db;

        public DbFileService(ISqlHelper db) => _db = db;

       
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

        public void Add(CSVModel item)
        {

            var sqliteCmd = _conn.CreateCommand();

            sqliteCmd.CommandText = "INSERT INTO File(NewsletterId, CustomerId, Username, X1) VALUES (" + item.NewsletterId 
                                    + "," + item.CustomerId + ", " + "' " + item.Username + "'" + "," + "' " + item.X1 + "'" + ") ;";

            sqliteCmd.ExecuteReader();

        }

         public void DeleteAll()
         {
            var sqliteCmd = _conn.CreateCommand();

            sqliteCmd.CommandText = "DELETE FROM File";

           sqliteCmd.ExecuteReader();

         }

         public CSVModels? GetCollection()
         {
            CreateConnection();

            CSVModels _db = new CSVModels();

            var sqliteCmd = _conn.CreateCommand();

            sqliteCmd.CommandText = "SELECT NewsletterId, CustomerId, Username, X1 FROM File";

            var sqliteDataReader = sqliteCmd.ExecuteReader();

            while (sqliteDataReader.Read())
            {
                long newsletterId = sqliteDataReader.GetInt64(0);
                long customerId = sqliteDataReader.GetInt64(1);
                string username = sqliteDataReader.GetString(2);
                string x1 = sqliteDataReader.GetInt32(3).ToString();

                _db.Items.Add(new CSVModel(newsletterId, customerId,
                  username, x1));
            }
           
        
           /*while (sqliteDataReader.Read())
            {
                Console.WriteLine(sqliteDataReader.GetInt64(0));
                Console.WriteLine(sqliteDataReader.GetInt64(1));
                Console.WriteLine(sqliteDataReader.GetString(2));
                Console.WriteLine(sqliteDataReader.GetInt32(3));
            }*/
            
            sqliteDataReader.Close();


            return _db;
         }

         

        public CSVModel? GetById(long newsletterId)
        {
            CreateConnection();

            var sqliteCmd = _conn.CreateCommand();

            sqliteCmd.CommandText = "SELECT * FROM File WHERE NewsletterId = " + newsletterId;


            var sqliteDataReader = sqliteCmd.ExecuteReader();

            CSVModels _db = new CSVModels();

            _db.Items.Clear();

        
            while (sqliteDataReader.Read())
            {
                long customerId = sqliteDataReader.GetInt64(1);
                string username = sqliteDataReader.GetString(2);
                string x1 = sqliteDataReader.GetInt32(3).ToString();

                _db.Items.Add(new CSVModel(newsletterId, customerId,
                  username, x1));
            }

            sqliteDataReader.Close();

            return _db.Items.FirstOrDefault(x => x.NewsletterId == newsletterId);
        }
        public void CloseConnection()
        {
            _conn.Close();
        }

 
    }
}
