using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
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

        public int i;


    }
}
