using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mazen.Models
{
    public class Company
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Company Name")]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
