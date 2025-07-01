using System.ComponentModel.DataAnnotations;

namespace DotNetWebApiProject.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
