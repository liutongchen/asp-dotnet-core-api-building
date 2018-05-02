using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class LocalMailServices : IMaiService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void send(string subject, string message) {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService");
            Debug.WriteLine($"subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}