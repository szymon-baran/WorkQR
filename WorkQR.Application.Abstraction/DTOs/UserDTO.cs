namespace WorkQR.Application
{
    public class UserDTO
    {
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
    }
}
