using System.ComponentModel.DataAnnotations;

namespace CommunityVLOG.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be in the range of 4 to 8 characters.")]
        public string Password { get; set; }
    }
}