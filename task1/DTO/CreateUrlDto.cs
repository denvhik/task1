using System.ComponentModel.DataAnnotations;

namespace task1.DTO
{
    public class CreateUrlDto
    {
        [Required(ErrorMessage = "The URL field is required.")]
        [RegularExpression(@"^(http|https)://[^\s/$.?#].[^\s]*$", ErrorMessage = "Invalid URL format")]
        public string URL { get; set; }
    }
}
