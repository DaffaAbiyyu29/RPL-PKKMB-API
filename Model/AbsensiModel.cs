using System.Data.SqlClient;

namespace PKKMB_API.Model
{
    public class AbsensiModel
    {
		public string abs_idabsensi { get; set; }
		public string abs_nim { get; set; }
		public string abs_nopendaftaran { get; set; }
		public DateTime abs_tglkehadiran { get; set; }
		public string abs_statuskehadiran { get; set; }
		public string abs_keterangan { get; set; }
		public string abs_idpkkmb { get; set; }
		public string abs_status { get; set; }
    }
}
