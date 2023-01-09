using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace MvcMovie.Models;

public class MovieViewModel
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

    [Display(Name = "Categories")]
    public MovieTypes? Categories { get; set; }

   

    public int i;
}