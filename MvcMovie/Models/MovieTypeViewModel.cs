using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class MovieTypeViewModel
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
    }
}
