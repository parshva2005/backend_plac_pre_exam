using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_plac_pre.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
