using AutoMapper;
using backend_plac_pre.Data;
using backend_plac_pre.DTO;
using backend_plac_pre.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_plac_pre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        #region Contructor
        private readonly STM_DbContext _context;
        private readonly IMapper _Mapper;
        public TicketController(STM_DbContext context, IMapper mapper)
        {
            _context = context;
            _Mapper = mapper;
        }
        #endregion

        #region Current User Role
        private string GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }
        #endregion

        #region Current User Id
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim) : 0;
        }
        #endregion

        #region Get Tickets
        [Authorize]
        [HttpGet("tickets")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var role = GetCurrentUserRole();
            var userId = GetCurrentUserId();
            if (role == "Manager")
            {
                var tickets = _context.Tickets.ToList();
                return Ok(tickets);
            }
            else if(role == "User")
            {
                var tickets = _context.Tickets.Where(t => t.CreatedByUserId == userId).ToList();
                return Ok(tickets);
            }
            else
            {
                var tickets = _context.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                return Ok(tickets);
            }
        }
        #endregion

        #region Create Ticket
        [Authorize]
        [HttpPost("tickets")]
        public async Task<ActionResult<Ticket>> CreateTicket([FromBody] Ticket_Dto ticketdto)
        {
            var role = GetCurrentUserRole();
            if (role == "Support") { 
                return Forbid("Supports are not allowed to create tickets.");
            }
            var userId = GetCurrentUserId();
            var ticket = _Mapper.Map<Ticket>(ticketdto);
            ticket.CreatedByUserId = userId;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
        #endregion

        #region Assign Ticket
        [Authorize(Roles = "Manager")]
        [HttpPut("tickets/{id}/assign")]
        public async Task<ActionResult<Ticket>> AssignTicket(int id, [FromBody] int assignedToUserId)
        {
            var role = GetCurrentUserRole();
            if (role != "Manager")
            {
                return Forbid("Only Managers can assign tickets.");
            }
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }
            ticket.AssignedToUserId = assignedToUserId;
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
        #endregion

        #region Update Ticket Status
        [Authorize(Roles = "Manager")]
        [HttpPut("tickets/{id}/status")]
        public async Task<ActionResult<Ticket>> UpdateTicketStatus(int id, [FromBody] string status)
        {
            var role = GetCurrentUserRole();
            if (role != "Manager")
            {
                return Forbid("Only Managers can update ticket status.");
            }
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }
            var oldStatus = ticket.Status;
            if (Enum.TryParse(status, out Ticket.TicketStatus newStatus))
            {
                var ticket_status_log = _Mapper.Map<Ticket_Status_Log>(new Ticket_Status_Log_Dto
                {
                    TicketId = ticket.Id,
                    OldStatus = oldStatus.ToString(),
                    NewStatus = newStatus.ToString(),
                    ChangedByUserId = GetCurrentUserId()
                });
                _context.Ticket_Status_Logs.Add(ticket_status_log);
                ticket.Status = newStatus;
                await _context.SaveChangesAsync();
                return Ok(ticket);
            }
            else
            {
                return BadRequest("Invalid status value.");
            }
        }
        #endregion

        #region Delete Ticket
        [Authorize(Roles = "Manager")]
        [HttpDelete("tickets/{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            var role = GetCurrentUserRole();
            if (role != "Manager")
            {
                return Forbid("Only Managers can delete tickets.");
            }
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}
