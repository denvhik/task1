using System.ComponentModel.DataAnnotations.Schema;

namespace task1.Models
{
    public class Url : BaseEntity
    {
        public string URL { get; set; }
        public string ShortUrl { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
