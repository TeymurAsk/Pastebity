using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastebin_api.Data;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pastebin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PastebinDbContext _context;
        public UsersController(PastebinDbContext context)
        {

            _context = context;

        }
        [HttpGet("{email}")]
        public User GetByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }
        // GET: api/<UsersController>
        [HttpGet]
        public List<User> Get()
        {
            return _context.Users.ToList();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _context.Users.Find(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post(User user)
        {
            if (user == null)
            {
                BadRequest("Item data is null");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            _context.Users.Find(id).Email = user.Email;
            _context.Users.Find(id).PasswordHash = user.PasswordHash;
            _context.Users.Find(id).TextblocksList = user.TextblocksList;
            _context.SaveChanges();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Users.Remove(_context.Users.Find(id));
        }
    }
}
