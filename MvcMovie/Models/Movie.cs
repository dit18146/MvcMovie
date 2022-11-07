using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class Movie
    {
     
        public int Id { get;}

        public string? Title { get;}

        public Movie(int id, string? title)
        {
            Id = id;
            Title = title;
        }

       

            /*[DataType(DataType.Date)]
            public DateTime ReleaseDate { get; set; }
            public string? Genre { get; set; }
            public decimal Price { get; set; }*/
        }
}
