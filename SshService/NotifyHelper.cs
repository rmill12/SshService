using System;

namespace SshService
{
    public static class NotifyHelper
    {
        private static string _ServerName = Environment.MachineName;
        private static string[] _ToAddresses
        {
            get
            {
                string[] arr = new string[Properties.Settings.Default.toEmails.Count];
                Properties.Settings.Default.toEmails.CopyTo(arr, 0);
                return arr;
            }
        }
        private static string _Subject
        {
            get
            {
                return _ServerName.ToUpper() + ": SSH Service";
            }
        }

        public static void SshClosed()
        {
            string body = "SSH connection CLOSED on " + _ServerName.ToUpper() + " to " + Properties.Settings.Default.host;
            EmailHelper.Send(_Subject, body, _ToAddresses);
        }
        public static void SshClosedReopening(int count, int limit)
        {
            string body = "SSH connection CLOSED on " + _ServerName.ToUpper() + " to " + Properties.Settings.Default.host;
            body += Environment.NewLine + "Attempt " + count.ToString() + " of " + limit.ToString() + " to reopen.";
            EmailHelper.Send(_Subject, body, _ToAddresses);
        }
        public static void SshOpened()
        {
            string body = "SSH connection successfully OPENED on " + _ServerName.ToUpper() + " to " + Properties.Settings.Default.host;
            EmailHelper.Send(_Subject, body, _ToAddresses);
        }
        public static void Started()
        {
            string body = "The SSH service has STARTED on " + _ServerName.ToUpper();
            EmailHelper.Send(_Subject, body, _ToAddresses);
        }
        public static void Stopped()
        {
            string body = "The SSH service has STOPPED on " + _ServerName.ToUpper();
            EmailHelper.Send(_Subject, body, _ToAddresses);
        }
    }
}
