# OTPService
## Setup instructions:

Instantiate Email_OTP_Module with a configured EmailConfig with known SMTP mail server provider.
```
EmailConfig emailConfig = new EmailConfig()
{
From = EmailConfigEmail,
SmtpServer = EmailConfigSmtpServer,
Username = EmailConfigEmail,
Password = EmailConfigPassword,
Port = EmailConfigPort,
};
_emailConfig = emailConfig;

Email_OTP_Module emailModule = new Email_OTP_Module(emailConfig);
```
Run _emailModule's functions and get response messages.
``` 
var result = _emailModule.Generate_OTP_Email(Email);

var result = _emailModule.Check_OTP(new MemoryStream(byteArray));
```