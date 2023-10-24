using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mazen.Models
{
    public class Category
    {
        // dotnet knows that this colomn is pk if :
        // it's name is just Id || the class name followed by id --> CategoryId || use [Key]
        [Key] // data annotation
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "The field Display Order must be between 1-100.")]
        public int DisplayOrder { get; set; }

    }
}
