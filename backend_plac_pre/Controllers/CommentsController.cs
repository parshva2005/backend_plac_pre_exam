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
    public class CommentsController : ControllerBase
    {
        #region Contructor
        private readonly STM_DbContext _context;
        private readonly IMapper _Mapper;
        public CommentsController(STM_DbContext context, IMapper mapper)
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

        #region Add Comment By Ticket Id
        [Authorize]
        [HttpPost("tickets/{id}/comments")]
        public async Task<IActionResult> AddCommentByTicketId(int id, [FromBody] Comment_Perameter_Dto comment)
        {
            var role = GetCurrentUserRole();
            var userId = GetCurrentUserId();
            if (role == "User")
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id && t.CreatedByUserId == userId);
                if (ticket == null)
                {
                    return NotFound("Ticket not found or you do not have permission to comment on this ticket.");
                }
                var ticket_comment = _Mapper.Map<Ticket_Coment>(new Comment_Dto
                {
                    Comment = comment.Comment,
                    TicketId = id,
                    CreedByUserId = userId
                });
                _context.Ticket_Comments.Add(ticket_comment);
                await _context.SaveChangesAsync();
                return Ok(ticket_comment);
            }
            if(role == "Support")
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id && t.AssignedToUserId == userId);
                if (ticket == null)
                {
                    return NotFound("Ticket not found or you do not have permission to comment on this ticket.");
                }
                var ticket_comment = _Mapper.Map<Ticket_Coment>(new Comment_Dto
                {
                    Comment = comment.Comment,
                    TicketId = id,
                    CreedByUserId = userId
                });
                _context.Ticket_Comments.Add(ticket_comment);
                await _context.SaveChangesAsync();
                return Ok(ticket_comment);
            }
            else
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }
                var ticket_comment = _Mapper.Map<Ticket_Coment>(new Comment_Dto
                {
                    Comment = comment.Comment,
                    TicketId = id,
                    CreedByUserId = userId
                });
                _context.Ticket_Comments.Add(ticket_comment);
                await _context.SaveChangesAsync();
                return Ok(ticket_comment);
            }
                
        }
        #endregion

        #region Get Comment By Ticket Id
        [Authorize]
        [HttpGet("ticket/{id}/comments")]
        public async Task<IActionResult> GetCommentByTicketId(int id)
        {
            var role = GetCurrentUserRole();
            var userId = GetCurrentUserId();
            if (role == "User")
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id && t.CreatedByUserId == userId);
                if (ticket == null)
                {
                    return NotFound("Ticket not found or you do not have permission to comment on this ticket.");
                }
                return Ok(ticket);
            }
            if (role == "Support")
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id && t.AssignedToUserId == userId);
                if (ticket == null)
                {
                    return NotFound("Ticket not found or you do not have permission to comment on this ticket.");
                }
                return Ok(ticket);
            }
            else
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }
                return Ok(ticket);
            }
        }
        #endregion

        #region Comment Update By Comment Id
        [Authorize]
        [HttpPut("comments/{id}")]
        public async Task<IActionResult> CommentUpdateByCommentId(int id, [FromBody] Comment_Perameter_Dto comment)
        {
            var role = GetCurrentUserRole();
            var userId = GetCurrentUserId();
            var ticket_comment = _context.Ticket_Comments.FirstOrDefault(c => c.Ticket_Coment_Id == id);
            if (ticket_comment == null)
            {
                return NotFound("Comment not found.");
            }
            if (role == "User" && ticket_comment.Created_By_UserId != userId)
            {
                return Forbid("You do not have permission to update this comment.");
            }
            if (role == "Support" && ticket_comment.Created_By_UserId != userId)
            {
                return Forbid("You do not have permission to update this comment.");
            }
            ticket_comment.Coment = comment.Comment;
            _context.Ticket_Comments.Update(ticket_comment);
            await _context.SaveChangesAsync();
            return Ok(ticket_comment);
        }
        #endregion

        #region Delete Comment By Comment Id
        [Authorize]
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteCommentByCommentId(int id)
        {
            var role = GetCurrentUserRole();
            var userId = GetCurrentUserId();
            var ticket_comment = _context.Ticket_Comments.FirstOrDefault(c => c.Ticket_Coment_Id == id);
            if (ticket_comment == null)
            {
                return NotFound("Comment not found.");
            }
            if (role == "User" && ticket_comment.Created_By_UserId != userId)
            {
                return Forbid("You do not have permission to delete this comment.");
            }
            if (role == "Support" && ticket_comment.Created_By_UserId != userId)
            {
                return Forbid("You do not have permission to delete this comment.");
            }
            _context.Ticket_Comments.Remove(ticket_comment);
            await _context.SaveChangesAsync();
            return Ok("Comment deleted successfully.");
        }
        #endregion
    }
}
