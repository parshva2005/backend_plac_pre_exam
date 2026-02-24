using AutoMapper;
using backend_plac_pre.Data;
using backend_plac_pre.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend_plac_pre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Constructor
        private readonly STM_DbContext _context;
        private readonly IMapper _Mapper;
        private readonly IConfiguration _configuration;
        public AuthController(STM_DbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _Mapper = mapper;
            _configuration = configuration;
        }
        #endregion

        //#region Register
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(Register_Dto request)
        //{
        //    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        //    if (existingUser != null)
        //    {
        //        return BadRequest("Email already in use.");
        //    }
        //    CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        //    var newUser = new Models.User
        //    {
        //        Name = request.Name,
        //        Email = request.Email,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        RoleId = 2 
        //    };
        //    _context.Users.Add(newUser);
        //    await _context.SaveChangesAsync();

        //    return Ok("Registration successful!");
        //}
        //#endregion

        //#region Create Password Hash
        //private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    using (var hmac = new HMACSHA512())
        //    {
        //        passwordSalt = hmac.Key;
        //        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        //    }
        //}
        //#endregion 

        #region Verify Password Hash
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<ActionResult<Auth_Response_Dto>> Login(User_Login_Dto login_dto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == login_dto.Email);
            if(user == null)
            {
                return BadRequest("User not found.");
            }
            if (!VerifyPasswordHash(login_dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Incorrect password.");
            }
            var token = GenerateJwtToken(user);
            return Ok(new Auth_Response_Dto
            {
                Token = token,
                User = _Mapper.Map<UserProfileDto>(user)
            });
        }
        #endregion

        #region Generate JWT Token
        private string GenerateJwtToken(Models.User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.roleName.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
