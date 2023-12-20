namespace PKKMB_API.Model
{
	public class ResponseModel
	{
		public int status { get; set; }
		public string? messages { get; set; }
		public object? data { get; set; }
	}
}
