using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OTPService.Models
{
	public static class Responses
	{
		private const string STATUS_EMAIL_OK = "email containing OTP has been sent successfully.";
		private const string STATUS_EMAIL_FAIL = "email address does not exist or sending to the email has failed.";
		private const string STATUS_EMAIL_INVALID = "email address is invalid.";

		private const string STATUS_OTP_OK = "OTP is valid and checked.";
		private const string STATUS_OTP_FAIL = "OTP is wrong.";
		private const string STATUS_OTP_FAIL_AFTER_MAX_TRIES = "OTP is wrong after 10 tries.";
		private const string STATUS_OTP_TIMEOUT = "timeout after 1 min.";

		public static string Convert(Email_OTP_Module.EMAIL_STATUS status) 
		{
			string reply = "";
			switch (status)
			{
				case Email_OTP_Module.EMAIL_STATUS.EMAIL_FAIL:
					reply = STATUS_EMAIL_FAIL;
					break;

				case Email_OTP_Module.EMAIL_STATUS.EMAIL_INVALID:
					reply = STATUS_EMAIL_INVALID;
					break;

				case Email_OTP_Module.EMAIL_STATUS.EMAIL_OK:
					reply = STATUS_EMAIL_OK;
					break;
				default:
					reply = STATUS_EMAIL_FAIL;
					break;
					
			}
			return reply;
		}
		public static string Convert(Email_OTP_Module.OTP_STATUS status)
		{
			string reply = "";
			switch (status)
			{
				case Email_OTP_Module.OTP_STATUS.OTP_FAIL:
					reply = STATUS_OTP_FAIL;
					break;

				case Email_OTP_Module.OTP_STATUS.OTP_TIMEOUT:
					reply = STATUS_OTP_TIMEOUT;
					break;

				case Email_OTP_Module.OTP_STATUS.OTP_OK:
					reply = STATUS_OTP_OK;
					break;
				case Email_OTP_Module.OTP_STATUS.OTP_FAIL_AFTER_MAX_TRIES:
					reply = STATUS_OTP_FAIL_AFTER_MAX_TRIES;
					break;
				default:
					reply = STATUS_OTP_FAIL;
					break;
			}
			return reply;
		}
	}
}
