using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcMovie.Models;

public class MovieType
{
    [Display(Name = "Id")]
    [Key]
    [Required]
    [Range(0, 1000)]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required]
    [MaxLength(20)]
    public string? Name { get; set; }

    public MovieType(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}