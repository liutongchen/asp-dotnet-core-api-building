using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class CloudMailServices : IMaiService
    {
        private string _mailTo = "123@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";
        public void send(string subject, string message) {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}