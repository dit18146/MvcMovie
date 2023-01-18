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

            sqliteCmd.CommandText = "INSERT INTO Newsletter(NewsletterId, CustomerId, Username, X1) VALUES (" + item.NewsletterId 
                                    + "," + item.CustomerId + ", " + "' " + item.Username + "'" + "," + "' " + item.X1 + "'" + ") ;";

            sqliteCmd.ExecuteReader();

        }

         public void Update(CSVModel item)
         {
            var sqliteCmd = _conn.CreateCommand();

            CSVModels _db = new CSVModels();

            sqliteCmd.CommandText = "UPDATE file SET NewsletterId = " + "'" + item.NewsletterId + "'" + ", CustomerId = " + "'" +
                                    item.CustomerId + "'" + "," + " Login = " + "'" + item.Username + "'" + ", X1 = " + "'" +
                                    item.X1 + "'" +
                                    "WHERE id = " + item.CustomerId + ";";

            var sqliteDataReader = sqliteCmd.ExecuteReader();

            while (sqliteDataReader.Read())
                _db.Items.Add(new CSVModel(sqliteDataReader.GetInt64(0), sqliteDataReader.GetInt64(1),
                    sqliteDataReader.GetString(2), sqliteDataReader.GetString(3)));

            sqliteDataReader.Close();
         }

         public CSVModels? GetCollection()
         {
            CreateConnection();

            CSVModels _db = new CSVModels();

            var sqliteCmd = _conn.CreateCommand();

            sqliteCmd.CommandText = "SELECT NewsletterId, CustomerId, Username, X1 FROM Newsletter";

            var sqliteDataReader = sqliteCmd.ExecuteReader();

           while (sqliteDataReader.Read())
               _db.Items.Add(new CSVModel(sqliteDataReader.GetInt64(0), sqliteDataReader.GetInt64(1),
                    sqliteDataReader.GetString(2), sqliteDataReader.GetString(3)));
        
           
            
            sqliteDataReader.Close();


            return _db;
         }

         

         /*public async Task<CSVModels?> GetCollectionAsync()
         {

                var query = await _db.QueryAsync<FileDto>(_sqlConnString,
                    "SELECT NewsletterId, CustomerId, Username, X1 FROM [File]",
                    commandType: CommandType.Text).ConfigureAwait(false);
                

                var records = new CSVModels();
                {
                    //Items = query.Value
                };
                return records;
         }*/

        public CSVModel? GetById(int? customerId)
        {
            CreateConnection();

            var sqliteCmd = _conn.CreateCommand();

            sqliteCmd.CommandText = "SELECT * FROM File WHERE id = " + customerId;


            var sqliteDataReader = sqliteCmd.ExecuteReader();

            CSVModels _db = new CSVModels();

            _db.Items.Clear();

        
            while (sqliteDataReader.Read())
                 _db.Items.Add(new CSVModel(sqliteDataReader.GetInt64(0), sqliteDataReader.GetInt64(1),
                    sqliteDataReader.GetString(2), sqliteDataReader.GetString(3)));

            sqliteDataReader.Close();

            return _db.Items.FirstOrDefault(x => x.CustomerId == customerId);
        }
        public void CloseConnection()
        {
            _conn.Close();
        }

 
    }
}
