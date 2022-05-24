using System.ComponentModel.DataAnnotations;

namespace Screechr.Model
{
    public class Screech
    {
        [Key]
        public long Id { get; set; }
        [Required, StringLength(1024)]
        public string Content { get; set; } = string.Empty;
        public long CreatorId { get; set; }
        public User Creator { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
