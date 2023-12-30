using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AbsensiController : Controller
	{
		private readonly AbsensiRepository _absenRepo;
		ResponseModel response = new ResponseModel();

		public AbsensiController(IConfiguration configuration)
		{
			_absenRepo = new AbsensiRepository(configuration);
		}

		[HttpGet("/GetAllAbsensi", Name = "GetAllAbsensi")]
		public IActionResult GetAllAbsensi()
		{
			try
			{
				response.status = 200;
				response.messages = "Berhasil";
				response.data = _absenRepo.getAllData();
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Gagal";
			}
			return Ok(response);
		}

		[HttpGet("/GetAbsensi", Name = "GetAbsensi")]
		public IActionResult GetAbsensi(string abs_idabsensi)
		{
			try
			{
				response.status = 200;
				response.messages = "Berhasil";
				response.data = _absenRepo.getData(abs_idabsensi);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Gagal, " + ex.Message;
			}
			return Ok(response);
		}

		[HttpPost("/TambahAbsensi", Name = "TambahAbsensi")]
		public IActionResult TambahAbsensi([FromBody] List<AbsensiModel> absensiList)
		{
			var result = _absenRepo.TambahAbsensi(absensiList);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/UpdateAbsensi", Name = "UpdateAbsensi")]
		public IActionResult UpdateAbsensi([FromBody] AbsensiModel absensi)
		{
			AbsensiModel absensi1 = new AbsensiModel();
			absensi1.abs_idabsensi = absensi.abs_idabsensi;
			absensi1.abs_nim = absensi.abs_nim;
			absensi1.abs_nopendaftaran = absensi.abs_nopendaftaran;
			absensi1.abs_tglkehadiran = absensi.abs_tglkehadiran;
			absensi1.abs_statuskehadiran = absensi.abs_statuskehadiran;
			absensi1.abs_keterangan = absensi.abs_keterangan;
			absensi1.abs_status = absensi.abs_status;

			try
			{
				response.status = 200;
				response.messages = "Berhasil";
				_absenRepo.ubahAbsensi(absensi);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Gagal, " + ex.Message;
			}
			return Ok(response);
		}
	}
}