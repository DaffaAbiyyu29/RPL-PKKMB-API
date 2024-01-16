using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class KelompokModel
	{
		public string kmk_idkelompok { get; set; }
		public string kmk_namakelompok { get; set; }
		public string kmk_nim { get; set; }
		public string kmk_idruangan { get; set; }
		public string kmk_idpkkmb { get; set; }
		public string kmk_status { get; set; }
	}
}


