﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPService.Models
{
	public class EmailConfig
	{
		public string From { get; set; } = null!;
		public string SmtpServer { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Username { get; set; } = null!;
		public int Port { get; set; }

	}
}
