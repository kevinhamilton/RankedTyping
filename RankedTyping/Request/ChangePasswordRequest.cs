namespace RankedTyping.Request
{
    public class ChangePasswordRequest
    {
        public string token { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}