
using OTPService.Models;
using System.IO;
using System.Net.Mail;
using OTPService.Services;

namespace OTPService
{
	/// <summary>
	/// Please run in Release Mode if you are using a ".dso.org.sg" domain email as the default filter email.
	/// Else change DEFAULT_EMAIL_DOMAIN constant to indictate the default domain.
	/// </summary>
	public class Email_OTP_Module
	{
		private const int OTP_VALID_DURATION = 60000; // 1 minute
		private const int MAX_TRIES = 10;
		/// <summary>
		/// Test scenario use personal email or set here.
		/// </summary>
#if DEBUG
		private const string DEFAULT_EMAIL_DOMAIN = "gmail.com";// ".dso.org.sg";
#elif RELEASE
        private const string DEFAULT_EMAIL_DOMAIN = ".dso.org.sg";
#endif

		private int tries = 0;
		private string _currentOTP = "";
		private string _senderEmail = "";

		private CancellationTokenSource cts;
		private EmailService _emailService;
		public Email_OTP_Module(EmailConfig emailConfig)
		{
			_emailService = new EmailService(emailConfig);
		}

		public enum EMAIL_STATUS
		{
			EMAIL_OK,
			EMAIL_FAIL,
			EMAIL_INVALID
		}
		public enum OTP_STATUS
		{
			OTP_OK,
			OTP_FAIL,
			OTP_FAIL_AFTER_MAX_TRIES,
			OTP_TIMEOUT
		}
		public void Start()
		{
			// Optional: initialize variables or resources
		}

		public void Close()
		{
			// Optional: clean up variables or resources
		}
		public void SetSenderEmail(string senderEmail)
		{
			_senderEmail = senderEmail;
		}

		public string Generate_OTP_Email(string userEmail)
		{
			if (!IsValidEmail(userEmail))
			{
				return Responses.Convert(EMAIL_STATUS.EMAIL_INVALID);
			}

			if (!userEmail.EndsWith(DEFAULT_EMAIL_DOMAIN))
			{
				return Responses.Convert(EMAIL_STATUS.EMAIL_INVALID);
			}

			_currentOTP = GenerateRandomOTP();

			string body = $"Your OTP Code is {_currentOTP}. The code is valid for 1 minute";
			string subject = $"OTP Code to log into";

			try
			{
				Send_Email(_senderEmail, userEmail, subject, body);
				return Responses.Convert(EMAIL_STATUS.EMAIL_OK);
			}
			catch
			{
				return Responses.Convert(EMAIL_STATUS.EMAIL_FAIL);
			}
		}
		public string Check_OTP(Stream input)
		{
			if (tries == 0)
				cts = new CancellationTokenSource(OTP_VALID_DURATION);

			Task<string> otpTask = Task.Run(() => ReadOTP(input, cts.Token));
			string enteredOTP = otpTask.Result;
			if (enteredOTP == Responses.Convert(OTP_STATUS.OTP_TIMEOUT))
			{
				return Responses.Convert(OTP_STATUS.OTP_TIMEOUT);
			}
			if (enteredOTP == _currentOTP)
			{
				return Responses.Convert(OTP_STATUS.OTP_OK);
			}

			tries++;

			if (tries > MAX_TRIES)
				return Responses.Convert(OTP_STATUS.OTP_FAIL_AFTER_MAX_TRIES);
			return Responses.Convert(OTP_STATUS.OTP_FAIL);
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
		private string GenerateRandomOTP()
		{
			Random rnd = new Random();
			return rnd.Next(100000, 999999).ToString();
		}

		private void Send_Email(string senderEmail, string receiverEmail, string subject, string body)
		{
			IEnumerable<string> receivers = new List<string>() { receiverEmail };
			var email = new Mail(receivers, senderEmail, subject, body);
			_emailService.SendEmail(email);

		}

		private string ReadOTP(Stream input, CancellationToken token)
		{
			using (var reader = new StreamReader(input))
			{
				try
				{
					while (!token.IsCancellationRequested)
					{
						if (reader.Peek() >= 0)
						{
							return reader.ReadLine();
						}
					}

					throw new OperationCanceledException();
				}
				catch (OperationCanceledException)
				{
					return Responses.Convert(OTP_STATUS.OTP_TIMEOUT);
				}
			}
		}
	}

}
