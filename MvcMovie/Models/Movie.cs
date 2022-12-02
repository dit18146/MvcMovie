namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; }

    public string? Title { get; }

    public string? Description { get; }

    public int? MovieTypeId { get; }

    public string? Name { get; }

    public MovieTypes Categories { get; set; }


    public Movie(int id, string? title, string description)
    {
        Id = id;

        Title = title;

        Description = description;
    }

    public Movie(int id, string? title, string description, int? movieTypeId)
    {
        Id = id;

        Title = title;

        Description = description;

        MovieTypeId = movieTypeId;
    }

    public Movie(int id, string? title, string description, string name)
    {
        Id = id;

        Title = title;

        Description = description;

        Name = name;
    }

    public Movie(MovieTypes categories) => Categories = categories;


    /*[DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }*/
}