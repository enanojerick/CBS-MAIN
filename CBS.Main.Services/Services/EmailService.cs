using System;
using System.IO;
using MimeKit;

using CBS.Common.Interface;
using CBS.Common.Services.Email;
using CBS.Service.Interfaces;
using CBS.Dto.ViewModel;


namespace CBS.Main.Services
{
    public class EmailService : IEmailService
    {
        private IEmailSender _EmailSender;

        public EmailService(IEmailSender EmailSender)
        {
            _EmailSender = EmailSender;
        }

        public bool SendCredentialToEmail(UserDto user, string emailtemplate)
        {
            try
            {
                var name = user.FirstName + " " + user.LastName;

                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(emailtemplate))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                string messagebody = string.Format(builder.HtmlBody, name.ToString(), user.Email.ToString(), user.RegisterPassword.ToString());

                EmailDto emaildto = new EmailDto()
                {
                    EmailBody = messagebody,
                    EmailSubject = "Welcome to CBS!",
                    EmailSender = "noreply.cbs@gmail.com",
                    EmailRecipient = user.Email
                };

                _EmailSender.SendEmail(emaildto);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool SendForgorPasswordRequestToEmail(UserDto user, string emailtemplate, string callbackurl)
        {
            try
            {
                var name = user.FirstName + " " + user.LastName;

                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(emailtemplate))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                string messagebody = string.Format(builder.HtmlBody, name.ToString(), callbackurl);

                EmailDto emaildto = new EmailDto()
                {
                    EmailBody = messagebody,
                    EmailSubject = "Welcome to CBS!",
                    EmailSender = "noreply.cbs@gmail.com",
                    EmailRecipient = user.Email
                };

                _EmailSender.SendEmail(emaildto);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
