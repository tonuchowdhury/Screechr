using System.ComponentModel.DataAnnotations;

namespace Screechr.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [Required, StringLength(80)]
        public string UserName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public string SecretToken { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.MinValue;
        public DateTime DateModified { get; set; } = DateTime.MinValue;
    }
}
