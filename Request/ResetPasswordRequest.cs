namespace RankedTyping.Request
{
    public class ResetPasswordRequest
    {
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
}