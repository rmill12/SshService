namespace SshService
{
    public static class EmailHelper
    {
        public static void Send(string subject, string body, string toEmail)
        {
            Send(subject, body, new string[] { toEmail });
        }
        public static void Send(string subject, string body, string[] toEmails)
        {
            Send(subject, body, toEmails, null);
        }
        public static void Send(string subject, string body, string[] toEmails, string[] ccEmails)
        {
            Send(subject, body, toEmails, ccEmails, null);
        }
        public static void Send(string subject, string body, string[] toEmails, string[] ccEmails, string[] bccEmails)
        {
            // Do what you gotta do to send emails!
        }
    }
}
