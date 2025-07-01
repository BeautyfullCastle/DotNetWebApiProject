using System.ComponentModel.DataAnnotations;

namespace DotNetWebApiProject.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
