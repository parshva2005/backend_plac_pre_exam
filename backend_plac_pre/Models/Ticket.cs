using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_plac_pre.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [MinLength(5)]
        public string Title { get; set; }
        [MinLength(10)]
        public string Description { get; set; }
        public enum TicketStatus {
            OPEN = 1,
            IN_PROGRESS = 2,
            ROLVED = 3,
            COSED = 4
        }
        public TicketStatus Status { get; set; } = TicketStatus.OPEN;
        public enum PriorityLevel {
            LOW = 1,
            MEDIUM = 2,
            HIGH = 3
        }
        public PriorityLevel Priority { get; set; } = PriorityLevel.MEDIUM;
        public int CreatedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }
    }
}
