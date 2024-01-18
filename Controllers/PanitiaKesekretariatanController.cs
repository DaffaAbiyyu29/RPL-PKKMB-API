using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PanitiaKesekretariatanController : Controller
	{
		private readonly PanitiaKesekretariatanRepository _kskRepo;
		private readonly IConfiguration _configuration;

		public PanitiaKesekretariatanController(IConfiguration configuration)
		{
			_kskRepo = new PanitiaKesekretariatanRepository(configuration);
			_configuration = configuration;
		}

		[HttpGet("/TampilKskAktif", Name = "TampilKskAktif")]
		public IActionResult TampilKskAktif(string ksk_idpkkmb)
		{
			var ksk = _kskRepo.TampilKskAktif(ksk_idpkkmb);
			try
			{
				if (ksk != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Panitia Kesekretariatan Aktif", Data = ksk });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Belum Ada Panitia Kesekretariatan Yang Terdaftar", Data = ksk });
				}
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Panitia Kesekretariatan", Data = ex.Message });
			}
		}

		[HttpGet("/TampilFasilitator", Name = "TampilFasilitator")]
		public IActionResult TampilFasilitator(string ksk_idpkkmb)
		{
			var ksk = _kskRepo.TampilFasilitator(ksk_idpkkmb);
			try
			{
				if (ksk != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Panitia Kesekretariatan Aktif", Data = ksk });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Belum Ada Panitia Kesekretariatan Yang Terdaftar", Data = ksk });
				}
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Panitia Kesekretariatan", Data = ex.Message });
			}
		}

		[HttpGet("/TampilKsk", Name = "TampilKsk")]
		public IActionResult TampilKsk(string ksk_idpkkmb)
		{
			var ksk = _kskRepo.TampilKsk(ksk_idpkkmb);
			try
			{
				if (ksk != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Panitia Kesekretariatan Aktif", Data = ksk });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Belum Ada Panitia Kesekretariatan Yang Terdaftar", Data = ksk });
				}
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Panitia Kesekretariatan", Data = ex.Message });
			}
		}

		[HttpGet("/TampilKskDraft", Name = "TampilKskDraft")]
		public IActionResult TampilKskDraft(string ksk_idpkkmb)
		{
			var ksk = _kskRepo.TampilKskDraft(ksk_idpkkmb);
			try
			{
				if (ksk != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Panitia Kesekretariatan Menunggu Verifikasi", Data = ksk });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Tidak Ada Panitia Kesekretariatan Yang Masih Menunggu Verifikasi", Data = ksk });
				}
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Panitia Kesekretariatan", Data = ex.Message });
			}
		}

		[HttpGet("/dataksk", Name = "DataKsk")]
		public IActionResult DataKsk(string ksk_nim)
		{
			PanitiaKesekretariatanModel ksk = _kskRepo.getData(ksk_nim);
			try
			{
				if (ksk != null)
				{
					return Ok(new { Status = 200, Messages = "Akun ditemukan", Role = "Panitia Kesekretariatan", Data = ksk });

				}
				else
				{
					// Account not found
					return NotFound(new { Status = 404, Messages = "Akun Tidak Ditemukan", Data = new Object() });
				}
			}
			catch (Exception ex)
			{
				// General error
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Login = " + ex.Message, Data = new Object() });
			}
		}

		[HttpPost("/daftarksk", Name = "DaftarKsk")]
		public IActionResult DaftarKsk([FromBody] PanitiaKesekretariatanModel ksk)
		{
			var result = _kskRepo.daftarKsk(ksk);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/ubahksk", Name = "Updateksk")]
		public IActionResult Updateksk([FromBody] PanitiaKesekretariatanModel ksk)
		{
			var result = _kskRepo.updateKsk(ksk);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/verifikasiKSK", Name = "verifikasiKSK")]
		public IActionResult verifikasiKSK([FromBody] List<string> ksk_nim)
		{
			var result = _kskRepo.verifikasiKSK(ksk_nim);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/nonAktifKsk", Name = "nonAktifKsk")]
		public IActionResult nonAktifKsk([FromBody] List<string> ksk_nim)
		{
			var result = _kskRepo.nonAktifKsk(ksk_nim);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}
	}
}
