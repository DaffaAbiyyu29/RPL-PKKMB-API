namespace PKKMB_API.Model
{
	public class EmailSettingModel
	{
		public string SmtpServer { get; set; }
		public int SmtpPort { get; set; }
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }
		public string SenderEmail { get; set; }
	}
}
