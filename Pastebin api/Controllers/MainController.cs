using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pastebin_api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pastebin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly S3Controller _s3controller;
        public MainController(S3Controller s3controller)
        {
            _s3controller = s3controller;
        }
        [HttpGet("{key}")]
        public async Task<string> Get(string key)
        {
            return await _s3controller.Get(key);
        }

        // POST api/<MainController>
        [HttpPost]
        public string Post(string content)
        {
            var key = "testing";
            _s3controller.Post(key, content);
            return key;
        }
    }
}
