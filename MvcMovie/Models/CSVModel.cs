using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcMovie.Models
{
    public class CSVModel
    {
        [Display(Name = "CustomerId")]
        [Required]
        public long CustomerId { get; set; }
        [Display(Name = "Login")]
        [Required]
        public string Login { get; set; }
        [Display(Name = "X1")]
        [Required]
        public string X1 { get; set; }

      


    }
}
