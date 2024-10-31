using Microsoft.EntityFrameworkCore;

namespace Pastebin_api.Data
{
    public class PastebinDbContext : DbContext
    {
        public PastebinDbContext(DbContextOptions<PastebinDbContext> options) : base(options)
        {

        }
        public List<TextBlock> TextBlocks { get; set; }
    }
}
