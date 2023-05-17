using task1.Models;

namespace task1.DTO
{
    public class CreateUserDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public int PhoneNumber { get; set; }

    }
}
