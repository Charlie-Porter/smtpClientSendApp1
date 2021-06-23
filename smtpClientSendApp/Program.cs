using System;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;

namespace SmtpClientSendApp
{
    public class SimpleAsynchronousExample
    {
        static bool mailSent = false;
        static string smtp_username = "";
        static string smtp_password = "";
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
                Console.WriteLine("Goodbye.");
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }
            mailSent = true;
        }

        public static void Main(string[] args)
        {
            // Command-line argument must be the SMTP host.
            try
            {
                    string Host = "";
                Console.Write("Enter smtp server: ");
                Host = Console.ReadLine();


                Console.WriteLine("#####################################################################################################################  Some SMTP servers require that the client be authenticated before the server sends email on its behalf. Type Yes if you want UseDefaultCredentials property to true when this SmtpClient object should, if requested by the server, authenticate using the default credentials of the currently logged on user.################################################################################################################### please press a key to continue ");
                Console.ReadLine();
                Console.WriteLine();

                string UseDefaultCredentials = "";
                Console.Write("To use UseDefaultCredentials please enter true or false ### Warning this is case sensitive ### : ");
                UseDefaultCredentials = Console.ReadLine();

                
                 if (UseDefaultCredentials == "false")
                {
                    
                    Console.Write("Enter smtp username: ");
                    smtp_username = Console.ReadLine();

                    
                    Console.Write("Enter smtp password: ");
                    smtp_password = Console.ReadLine();
                }

              

                int Port = 0;
                Console.Write("Enter smtp port: ");
                Port = Convert.ToInt32(Console.ReadLine()); ;

                bool EnableSsl = false;
                Console.Write("To enable SSL please enter true or false ### Warning this is case sensitive ### : ");
                EnableSsl = Convert.ToBoolean(Console.ReadLine());


                SmtpClient client = new SmtpClient();
                client.Host = Host;
                client.Port = Port;
                client.EnableSsl = EnableSsl;
                

                if (UseDefaultCredentials == "true") {
                    client.UseDefaultCredentials = true;
                }
                else
                 if (UseDefaultCredentials == "false")
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtp_username, smtp_password); 
                }

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
                message.Subject = "Test using SmtpClient";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                // Set the method that is called back when the send operation ends.
                client.SendCompleted += new
                SendCompletedEventHandler(SendCompletedCallback);
                // The userState can be any object that allows your callback
                // method to identify this send operation.
                // For this example, the userToken is a string constant.
                string userState = "test message1";
           
                client.SendAsync(message, userState);
                Console.WriteLine("Sending message... ");
                string answer = Console.ReadLine();
                // If the user canceled the send, and mail hasn't been sent yet,
                // then cancel the pending operation.
                if (answer.StartsWith("c") && mailSent == false)
                {
                    client.SendAsyncCancel();
                }
                // Clean up.
                message.Dispose();
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine("The following SMTP exception occurred: "+ ex);
                Console.ReadKey();
            }
          

        }
    }

  
}
