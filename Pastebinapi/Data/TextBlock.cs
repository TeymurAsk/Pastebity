using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pastebin_api.Data
{
    public class TextBlock
    {
        [Key]
        [StringLength(80)]
        public string Link { get; set; } = string.Empty;
        [Required]
        public DateTime ExpirationDate { get; set; }

    }
}
