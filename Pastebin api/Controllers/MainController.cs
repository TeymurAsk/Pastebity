using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pastebin_api.Data;
using Pastebin_api.Services;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pastebin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly S3Controller _s3controller;
        private readonly HashGenerator _hashGenerator;
        private readonly PastebinDbContext _context;
        private readonly RedisService _redisService;
        public MainController(S3Controller s3controller, HashGenerator hashgenerator, PastebinDbContext dbContext, RedisService redisService)
        {
            _s3controller = s3controller;
            _hashGenerator = hashgenerator;
            _context = dbContext;
            _redisService = redisService;
        }
        [HttpGet("{key}")]
        public async Task<string> Get(string key)
        {
            return await _s3controller.Get(key);
        }

        // POST api/<MainController>
        [HttpPost]
        public string Post(string content, int hours)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                Unauthorized("User ID claim not found in the token.");
                return "You're unauthorized, how did you even got here?";
            }

            var id = userIdClaim.Value.ToString();
            var key = _hashGenerator.GenerateUUID();
            _context.Users.Find(int.Parse(id)).TextblocksList.Add(key);
            _context.SaveChanges();

            TextBlock block = new TextBlock();
            block.Link = key;
            block.ExpirationDate = DateTime.UtcNow.AddHours(hours);

            _context.TextBlocks.Add(block);
            _context.SaveChanges();

            _redisService.ScheduleTask(key, DateTime.UtcNow.AddHours(hours));

            _s3controller.Post(key, content);
            
            return key;
        }
        [HttpDelete]
        public void Delete(string key)
        {
            _context.TextBlocks.Remove(_context.TextBlocks.Find(key));
            _context.SaveChanges();
        }
    }
}
