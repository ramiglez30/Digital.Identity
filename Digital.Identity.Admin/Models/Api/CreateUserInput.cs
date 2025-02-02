﻿using System.ComponentModel.DataAnnotations;

namespace Digital.Identity.Admin.Models.Api
{
    public class CreateUserInput
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
