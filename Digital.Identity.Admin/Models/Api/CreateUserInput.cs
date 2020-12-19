namespace Digital.Identity.Admin.Models.Api
{
    public class CreateUserInput
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
