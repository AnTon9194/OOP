using System;

namespace IndependentWork6.Notifications
{
    public class SmsNotifier : INotifier
    {
        public string PhoneNumber { get; set; }

        public SmsNotifier(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void Send(string message)
        {
            Console.WriteLine($"[SMS -> {PhoneNumber}]: {message}");
        }
    }
}
