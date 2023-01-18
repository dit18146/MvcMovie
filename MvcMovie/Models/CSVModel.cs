using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcMovie.Models
{
    public class CSVModel
    {
        [Display(Name = "NewsletterId")]
        //[Required]
        public long NewsletterId { get; set; }

        [Display(Name = "CustomerId")]
        //[Required]
        public long CustomerId { get; set; }
        [Display(Name = "Login")]
        //[Required]
        public string Username { get; set; }
        [Display(Name = "X1")]
        //[Required]
        public string X1 { get; set; }

      
        public CSVModel(long newsletterId, long customerId, string username, string x1) 
        {
            NewsletterId = NewsletterId;
            CustomerId = customerId;
             Username = username;
            X1 = x1;
        }

        public CSVModel(long newsletterId) 
        {

        }
        public CSVModel() 
        {

        }
    }
}
