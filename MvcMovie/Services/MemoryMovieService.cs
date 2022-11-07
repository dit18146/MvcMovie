using MvcMovie.Models;

namespace MvcMovie.Services
{
    public class MemoryMovieService : IMovieService
    {

        private Movies _db; 
        public MemoryMovieService()
        {
            _db = CreateDb(50);
        }

        public void Add(Movie item)
        {
            _db.Items.Add(item);
        }

        public void Delete(Movie item)
        {
            _db.Items.Remove(item);
        }

        public Movie GetById(int? id)
        {
            return  _db.Items.FirstOrDefault(x => x.Id == id);
            
        }

        public Movies GetCollection()
        {
            return _db;
        }

        public void Update(Movie item)
        {
            _db.Items[item.Id] = item;
        }

        private Movies CreateDb(int size)
        {

            var db = new Movies();

            for (int i = 0; i < size; i++)
            {
                db.Items.Add(new Movie(i, "Star Wars " + (i + 1)));
            }

            return db;

        }
    }
}
