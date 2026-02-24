using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_plac_pre.Models
{
    public class Ticket_Status_Log
    {
        [Key]
        public int Ticket_Status_Log_Id { get; set; }
        public int Ticket_Id { get; set; }
        [Required]
        public string Old_Status { get; set; }
        [Required]
        public string New_Status { get; set; }
        public DateTime Changed_At { get; set; } = DateTime.UtcNow;
        public int Changed_By_UserId { get; set; }

        [ForeignKey("Ticket_Id")]
        public virtual Ticket Ticket { get; set; }
        [ForeignKey("Changed_By_UserId")]
        public virtual User User { get; set; }
    }
}
