using Microsoft.AspNetCore.Mvc;
using Pastebin_api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pastebin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly S3Service _s3Service;
        private readonly RedisService _redisService;
        public S3Controller(S3Service s3Service, RedisService redisService)
        {
            _s3Service = s3Service;
            _redisService = redisService;
        }
        // GET api/<S3Controller>/5
        [HttpGet("{key}")]
        public async Task<string> Get(string key)
        {
            var text = _redisService.GetData<string>($"text{key}");
            if (text != null)
            {
                return text;
            }
            text = await _s3Service.GetTextAsync(key);
            _redisService.SetData($"text{key}", text);
            return text;
        }

        // POST api/<S3Controller>
        [HttpPost]
        public void Post(string key,string content)
        {
            _s3Service.UploadTextAsync(key, content).Wait();
        }

        // DELETE api/<S3Controller>/5
        [HttpDelete("{key}")]
        public void Delete(string key)
        {
            _s3Service.DeleteTextAsync(key);
        }
    }
}
