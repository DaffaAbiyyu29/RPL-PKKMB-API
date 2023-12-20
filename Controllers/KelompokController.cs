using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	public class KelompokController : Controller
	{
		private readonly KelompokRepository _kelRepo;
		private readonly IConfiguration _configuration;

		public KelompokController(IConfiguration configuration)
		{
			_kelRepo = new KelompokRepository(configuration);
			_configuration = configuration;
		}

		[HttpGet("/GetAllKelompok", Name = "GetAllKelompok")]
		public IActionResult GetAllKelompok()
		{
			try
			{
				var kel = _kelRepo.TampilKelompok();

				if (kel != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menambah Kelompok", Data = kel });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Gagal Menambah Kelompok", Data = kel });
				}
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Panitia Kesekretariatan", Data = ex.Message });
			}
		}

		[HttpGet("/GetKelompok", Name = "GetKelompok")]
		public IActionResult GetKelompok(string kmk_idkelompok)
		{
			KelompokModel kel = _kelRepo.getData(kmk_idkelompok);
			try
			{
				if (kel != null)
				{
					return Ok(new { Status = 200, Messages = "Kelompok ditemukan", Data = kel });

				}
				else
				{
					// Account not found
					return NotFound(new { Status = 404, Messages = "Kelompok Tidak Ditemukan", Data = new Object() });
				}
			}
			catch (Exception ex)
			{
				// General error
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menambahkan Kelompok = " + ex.Message, Data = new Object() });
			}
		}

		[HttpPost("/TambahKelompok", Name = "TambahKelompok")]
		public IActionResult TambahKelompok([FromBody] KelompokModel kel)
		{
			var result = _kelRepo.TambahKelompok(kel);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/UbahKelompok", Name = "UbahKelompok")]
		public IActionResult UbahKelompok([FromBody] KelompokModel kel)
		{
			var result = _kelRepo.UpdateKelompok(kel);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}
	}
}

