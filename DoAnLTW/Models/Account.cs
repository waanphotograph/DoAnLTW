using System.ComponentModel.DataAnnotations;
namespace DoAnLTW.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Role { get; set; }
    }
}
