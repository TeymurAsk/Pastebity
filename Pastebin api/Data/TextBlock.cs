using System.ComponentModel.DataAnnotations;

namespace Pastebin_api.Data
{
    public class TextBlock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Text { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

    }
}
