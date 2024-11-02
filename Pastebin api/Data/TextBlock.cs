using System.ComponentModel.DataAnnotations;

namespace Pastebin_api.Data
{
    public class TextBlock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Link { get; set; } = string.Empty;
        [Required]
        public DateTime ExpirationDate { get; set; }

    }
}
