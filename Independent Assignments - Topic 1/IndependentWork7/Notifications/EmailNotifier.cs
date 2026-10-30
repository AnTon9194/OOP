using System;

namespace IndependentWork6.Notifications
{
    public class EmailNotifier : INotifier
    {
        public string EmailAddress { get; set; }

        public EmailNotifier(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public void Send(string message)
        {
            Console.WriteLine($"[Email -> {EmailAddress}]: {message}");
        }
    }
}
