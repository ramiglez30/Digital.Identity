using System.ComponentModel.DataAnnotations;

namespace Digital.Identity.Admin.Models.Api
{
    public class EditUserInput
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
