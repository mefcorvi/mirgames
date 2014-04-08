namespace MirGames.Infrastructure.Notifications
{
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    /// <summary>
    /// The notification manager.
    /// </summary>
    internal sealed class NotificationManager : INotificationManager
    {
        /// <summary>
        /// The template processor.
        /// </summary>
        private readonly ITemplateProcessor templateProcessor;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationManager" /> class.
        /// </summary>
        /// <param name="templateProcessor">The template processor.</param>
        /// <param name="settings">The settings.</param>
        public NotificationManager(ITemplateProcessor templateProcessor, ISettings settings)
        {
            Contract.Requires(templateProcessor != null);
            this.templateProcessor = templateProcessor;
            this.settings = settings;
        }

        /// <inheritdoc />
        public void SendNotification<T>(string recipient, T notification) where T : Notification
        {
            var messageBody = this.templateProcessor.Process(notification.Body, notification, typeof(T).FullName + ".Body");
            var messageTitle = this.templateProcessor.Process(notification.Title, notification, typeof(T).FullName + ".Title");

            var pickupDirectory = this.settings.GetValue<string>("Smtp.PickupDirectory", null);

            var smtpMail = new SmtpClient();
             
            if (!string.IsNullOrWhiteSpace(pickupDirectory))
            {
                smtpMail.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpMail.PickupDirectoryLocation = pickupDirectory;
            }
            else
            {
                smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpMail.Host = this.settings.GetValue<string>("Smtp.Host");
                smtpMail.Port = this.settings.GetValue<int>("Smtp.Port");
                smtpMail.EnableSsl = true;
                smtpMail.Timeout = 50000;

                var smtpLogin = this.settings.GetValue<string>("Smtp.Login");
                var smtpPassword = this.settings.GetValue<string>("Smtp.Password");

                smtpMail.UseDefaultCredentials = false;
                smtpMail.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
            }

            var message = new MailMessage("info@mirgames.ru", recipient)
                {
                    Body = messageBody,
                    IsBodyHtml = true,
                    Subject = messageTitle,
                    SubjectEncoding = Encoding.GetEncoding(1251)
                };

            smtpMail.Send(message);
        }
    }
}
