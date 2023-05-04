namespace WorkQR.Application
{
    public class RegistrationEmailDTO
    {
        public string FullName { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string MailTo { get; set; } = ""; 
        public string VerificationCode { get; set; } = "";
    }
}
