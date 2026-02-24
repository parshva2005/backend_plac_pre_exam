using AutoMapper;
using backend_plac_pre.Data;
using backend_plac_pre.DTO;
using backend_plac_pre.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace backend_plac_pre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Constructor
        private readonly STM_DbContext _context;
        private readonly IMapper _Mapper;
        public UserController(STM_DbContext context, IMapper mapper)
        {
            _context = context;
            _Mapper = mapper;
        }
        #endregion

        #region Add User
        [Authorize(Roles = "Manager")]
        [HttpPost("users")]
        public async Task<IActionResult> AddUser(Register_Dto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already in use.");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var newUser = _Mapper.Map<Models.User>(request);
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;
            newUser.RoleId = request.RoleId;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Registration successful!");
        }
        #endregion

        #region Create Password Hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion

        #region Get Users
        [Authorize(Roles = "Manager")]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    RoleId = u.RoleId
                })
                .ToListAsync();
        }
        #endregion

    }
}
