namespace WorkQR.Application
{
    public class UserDTO
    {
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public long Expiration { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
