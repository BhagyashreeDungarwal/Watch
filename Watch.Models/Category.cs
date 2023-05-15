using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Watch.Models
{
    public class Category
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be 1 to 100!!")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
