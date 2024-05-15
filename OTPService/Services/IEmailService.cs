using OTPService.Models;

namespace OTPService.Services
{
	public interface IEmailService
	{
		void SendEmail(Mail email);
	}
}
