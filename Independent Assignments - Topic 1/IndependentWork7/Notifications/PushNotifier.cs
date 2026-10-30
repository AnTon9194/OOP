using System;

namespace IndependentWork6.Notifications
{
    public class PushNotifier : INotifier
    {
        public string DeviceToken { get; set; }

        public PushNotifier(string deviceToken)
        {
            DeviceToken = deviceToken;
        }

        public void Send(string message)
        {
            Console.WriteLine($"[Push -> {DeviceToken}]: {message}");
        }
    }
}
