using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_plac_pre.Models
{
    public class Ticket_Coment
    {
        [Key]
        public int Ticket_Coment_Id { get; set; }
        public int Ticket_Id { get; set; }
        public string Coment { get; set; }
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public int Created_By_UserId { get; set; }

        [ForeignKey("Ticket_Id")]
        public virtual Ticket Ticket { get; set; }
        [ForeignKey("Created_By_UserId")]
        public virtual User User { get; set; }
    }
}
