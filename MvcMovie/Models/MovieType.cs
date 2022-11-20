namespace MvcMovie.Models
{
    public class MovieType
    {
        public int Id { get; set;}

        public string? Name { get; set; }

        public MovieType(int id, string? name)
        {
            Id = id;
            Name = name;
        }
    }
}
