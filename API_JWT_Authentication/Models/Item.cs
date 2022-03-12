using System.ComponentModel.DataAnnotations;

namespace API_JWT_Authentication.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public bool Done { get; set; }
    }
}
