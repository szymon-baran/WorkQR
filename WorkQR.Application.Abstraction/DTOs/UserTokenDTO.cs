namespace WorkQR.Application
{
    public class UserTokenDTO
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public long Expiration { get; set; }
    }
}
