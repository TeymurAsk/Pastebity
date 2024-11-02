using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pastebin_api.Data;
using Pastebin_api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pastebin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly S3Controller _s3controller;
        private readonly HashGenerator _hashGenerator;
        private readonly PastebinDbContext _context;
        public MainController(S3Controller s3controller, HashGenerator hashgenerator, PastebinDbContext dbContext)
        {
            _s3controller = s3controller;
            _hashGenerator = hashgenerator;
            _context = dbContext;
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
            var key = _hashGenerator.GenerateUUID();
            TextBlock block = new TextBlock();
            block.Link = key;
            block.ExpirationDate = DateTime.UtcNow.AddHours(hours);
            _context.TextBlocks.Add(block);
            _context.SaveChanges();
            _s3controller.Post(key, content);
            return key;
        }
    }
}
