using backend_plac_pre.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_plac_pre.Data
{
    public class STM_DbContext : DbContext
    {
        public STM_DbContext(DbContextOptions<STM_DbContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Ticket_Coment> Ticket_Comments { get; set; }
        public DbSet<Ticket_Status_Log> Ticket_Status_Logs { get; set; }

    }
}
