using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pastebin_api.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Column("Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("Textblocks")]
        public List<string> TextblocksList { get; set; }
    }
}
