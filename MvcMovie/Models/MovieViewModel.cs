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


        [Display(Name = "Description")]
        [MaxLength(100)]
        public string? Description { get; set; }


        [Display(Name = "MovieTypeId")]
        [Range(0, 1000)]
        public int? MovieTypeId { get; set; }



        public int i;


    }
}
