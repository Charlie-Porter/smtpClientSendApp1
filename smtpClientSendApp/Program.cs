using System;
using System.Net.Mail;
using System.ComponentModel;

namespace SmtpClientSendApp
{
    public class SimpleAsynchronousExample
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        public static void Main(string[] args)
        {
            // Command-line argument must be the SMTP host.

            string Host = "";
            Console.Write("Enter smtp server: ");
            Host = Console.ReadLine();

            int Port = 0;
            Console.Write("Enter smtp port: ");
            Port = Convert.ToInt32(Console.ReadLine()); ;

            bool EnableSsl = false;
            Console.Write("Ssl enabled? true or false : ");
            EnableSsl = Convert.ToBoolean(Console.ReadLine());


            SmtpClient client = new SmtpClient();
            client.Host = Host;
            client.Port = Port;
            client.EnableSsl = EnableSsl;
            client.UseDefaultCredentials = true;

            // Create a mailing address that includes a UTF8 character
            // in the display name.
            string fr = "";
            Console.Write("From Email Address: ");
            fr = Console.ReadLine();
            MailAddress from = new MailAddress(fr);

            string to = "";
            Console.Write("To Email Address: ");
            to = Console.ReadLine();

            // Set destinations for the email message.
            MailAddress To = new MailAddress(to);
            // Specify the message content.
            MailMessage message = new MailMessage(from, To);
            message.Body = "This is a test email message sent by TEST console application. ";

            message.Body += DateTime.UtcNow;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test date using SmtpClient";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            if (answer.StartsWith("c") && mailSent == false)
            {
                client.SendAsyncCancel();
            }
            // Clean up.
            message.Dispose();
            Console.WriteLine("Goodbye.");
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

        }
    }

}
