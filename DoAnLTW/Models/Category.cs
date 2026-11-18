using System.ComponentModel.DataAnnotations;
namespace DoAnLTW.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public string DanhmucMota { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
