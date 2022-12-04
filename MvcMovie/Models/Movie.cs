namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? MovieTypeId { get; set; }

    public string? Name { get; set; }

    public MovieTypes Categories { get; set; } = new MovieTypes();


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
}