using Microsoft.AspNetCore.Mvc;
using HW1.BL;
using System.Text;
using System.Security.Cryptography;
using HW1.DAL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET api/Movies/All

        [HttpGet("All")]
        public IActionResult GetAllUsers()
        {
            Users user = new Users();
            return Ok(user.GetAllUsers());
        }

        // GET api/Users/{id}/active
        [HttpPut("{id}/active")]
        public IActionResult UpdateUserActiveStatus(int id, bool active)
        {
            Users user = new Users { Id = id, Active = active };
            bool result = user.UpdateActiveStatus(id,active);
            if (result)
                return Ok(new { message = "User status updated." });
            return BadRequest("Failed to update user status.");
        }

        // GET: api/<UsersController>

        [HttpGet("AllActive")]
        public IActionResult GetAllUsers([FromQuery] int currentUserId)
        {
            Users u = new Users();
            var users = u.GetAllActiveUsers(currentUserId);
            return Ok(users);
        }


        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Users user)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                if (user.Password.Length < 30)
                {
                    user.Password = HashPassword(user.Password);
                }
            }
            else
            {
                var db = new DBservices();
                var existingUser = db.Login(user.Email, null);
                if (existingUser == null)
                    return NotFound(new { message = "User not found." });

                user.Password = existingUser.Password;
            }

            Users updatedUser = user.UpdateUser(id);

            if (updatedUser != null)
            {
                return Ok(updatedUser); // ❗ מחזיר את המשתמש המעודכן, כולל סיסמה מוצפנת
            }

            return NotFound(new { message = "User not found." });
        }





        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (new Users().DeleteUser(id))
                return Ok(new { message = "User deleted successfully." });

            return NotFound(new { message = "User not found." });
        }

        // POST api/users/register

        [HttpPost("register")]
        public IActionResult Register([FromBody] Users newUser)
        {
            
            newUser.Password = HashPassword(newUser.Password);

            if (newUser.Register())
            {
                return Ok(new { message = "User registered successfully", newUser.Email });
            }

            return Conflict("❌ User with this email already exists.");
        }

        // POST api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest lr)
        {
            string hashedPassword = HashPassword(lr.Password);

            Users user = new Users();
            Users foundUser = user.Login(lr.Email, hashedPassword);

            if (foundUser!=null)
            {
                return Ok(foundUser);
            }

            return Unauthorized("Invalid email or password.");

        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
