using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pastebin_api.Data;
using Pastebin_api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pastebin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly HashGenerator _hashGenerator;
        private readonly PastebinDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(AuthService authService, IHttpContextAccessor httpContextAccessor, HashGenerator hashGenerator, PastebinDbContext context)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _hashGenerator = hashGenerator;
            _context = context;
        }
        /// <summary>
        /// Login into the system to use Main endpoints
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Auth/login
        ///     {
        ///         "email": "testemail@gmail.com",
        ///         "password": "testpassword",
        ///     }
        /// </remarks>
        /// <returns>The key for the content</returns>
        /// <response code="400">Please fill all of the fields</response>
        /// <response code="500">Incorrect credentials</response>
        // POST api/<AuthController>
        [HttpPost("login")]
        public void LoginUser(string email, string password)
        {
            _authService.Login(email, password, _httpContextAccessor.HttpContext);
        }
        /// <summary>
        /// Register in the system to use Main endpoints
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Auth/login
        ///     {
        ///         "email": "testemail@gmail.com",
        ///         "password": "testpassword",
        ///     }
        /// </remarks>
        /// <returns>The key for the content</returns>
        /// <response code="400">Please fill all of the fields</response>
        // POST api/<AuthController>
        [HttpPost("register")]
        public void RegisterUser(string email, string password)
        {
            // TODO add user in db
            var newuser = new User
            {
                Email = email,
                PasswordHash = _hashGenerator.Generate(password),
                TextblocksList = new List<string>(),
            };
            if (newuser == null)
            {
                BadRequest("Please fill form properly, the app wasn't build for manual testing");
            }
            _context.Users.Add(newuser);
            _context.SaveChanges();
            _authService.Login(email, password, _httpContextAccessor.HttpContext);
        }
    }
}
