using System.ComponentModel.DataAnnotations;

namespace backend_plac_pre.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public enum RoleName
        {
            Manager = 1,
            User = 2,
            Support = 3
        }
        public RoleName roleName { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
