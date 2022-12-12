using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcMovie.Models;

public class Movie
{
    [Display(Name = "Id")]
    [Key]
    [Required]
    [Range(0, 1000)]
    public int Id { get; set; }

    [Display(Name = "Title")]
    [Required]
    [MaxLength(20)]
    public string? Title { get; set; }


    [Display(Name = "Description")]
    [Required]
    [MaxLength(100)]
    public string? Description { get; set; }

    [Display(Name = "MovieTypeId")]
    [Range(1, 100)]
    [Required]
    public int? MovieTypeId { get; set; }

    public string? Name { get; set; }

    [Display(Name = "Categories")]
    public MovieTypes Categories { get; set; } = new MovieTypes();

    public Movie() { }

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