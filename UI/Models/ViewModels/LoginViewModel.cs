namespace UI.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsPersistent { get; set; }
        public string ReturnUrl { get; set; }
    }
}
