namespace UI.Models.ViewModels
{
    public class VerifyPhoneNumberViewModel
    {
        public string VerifyCode { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }
    }
}
