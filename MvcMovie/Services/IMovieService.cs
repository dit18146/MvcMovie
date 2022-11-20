using Microsoft.Data.SqlClient;
using MvcMovie.Models;

namespace MvcMovie.Services
{
    public interface IMovieService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Movie? GetById(int? id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Movies? GetCollection();

        void Add(Movie item);

        void Update(Movie item);

        void Delete(Movie item);

        bool CheckIfExists(int id);

        //void Add_Category(Movie item);

        //void Update_Category(Movie item);

        void ClearDatabase();

        void CloseConnection();
    }
}
