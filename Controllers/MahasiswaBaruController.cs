﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;
using PKKMB_API.Repository;
using System.Data;

namespace PKKMB_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Mahasiswa")]
	public class MahasiswaBaruController : Controller
	{
		private readonly MahasiswaBaruRepository _mhsBaruRepo;
		private readonly IConfiguration _configuration;

		public MahasiswaBaruController(IConfiguration configuration)
		{
			_mhsBaruRepo = new MahasiswaBaruRepository(configuration);
			_configuration = configuration;
		}

		[HttpGet("/getallmahasiswa", Name = "GetAllMahasiswa")]
		public IActionResult GetAllMahasiswa(string mhs_idpkkmb)
		{
			try
			{
				var mhsBaru = _mhsBaruRepo.getAllData(mhs_idpkkmb);
				if (mhsBaru != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Mahasiswa", Data = mhsBaru });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data Mahasiswa Tidak Ditemukan", Data = mhsBaru });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Mahasiswa", Data = ex.Message });
			}
		}

		[HttpGet("/datamahasiswa", Name = "DataMahasiswa")]
		//[Authorize(Roles = "Mahasiswa")]
		public IActionResult DataMahasiswa(string mhs_nopendaftaran)
		{
			MahasiswaBaruModel mhsBaru = _mhsBaruRepo.getData(mhs_nopendaftaran);
			try
			{
				if (mhsBaru != null)
				{
					return Ok(new { Status = 200, Messages = "Akun ditemukan", Role = "Mahasiswa Baru", Data = mhsBaru });

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

		[HttpPost("/daftarmahasiswa", Name = "DaftarMahasiswaBaru")]
		public IActionResult DaftarMahasiswaBaru([FromBody] MahasiswaBaruModel mhsBaru)
		{
			var result = _mhsBaruRepo.daftarMahasiswa(mhsBaru);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/ubahmahasiswa", Name = "UpdateMahasiswaBaru")]
		public IActionResult UpdateMahasiswaBaru([FromBody] MahasiswaBaruModel mhsBaru)
		{
			var result = _mhsBaruRepo.updateMahasiswa(mhsBaru);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/StatusKelulusanMahasiswa", Name = "StatusKelulusanMahasiswa")]
		public IActionResult StatusKelulusanMahasiswa([FromBody] string nopendaftaran)
		{
			var result = _mhsBaruRepo.statusKelulusan(nopendaftaran);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/EvaluasiMahasiswa", Name = "EvaluasiMahasiswa")]
		public IActionResult EvaluasiMahasiswa([FromBody] MahasiswaBaruModel mhsBaru)
		{
			var result = _mhsBaruRepo.evaluasiMahasiswa(mhsBaru);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpGet("/TampilMahasiswaTanpaKelompok", Name = "TampilMahasiswaTanpaKelompok")]
		public IActionResult TampilMahasiswaTanpaKelompok()
		{
			try
			{
				var mhsBaru = _mhsBaruRepo.TampilMahasiswaTanpaKelompok();
				if (mhsBaru != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Mahasiswa", Data = mhsBaru });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data Mahasiswa Tidak Ditemukan", Data = mhsBaru });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Mahasiswa", Data = ex.Message });
			}
		}

		[HttpPut("/PengelompokkanMahasiswa", Name = "PengelompokkanMahasiswa")]
		public IActionResult PengelompokkanMahasiswa([FromBody] List<string> mhs_nopendaftaran, string mhs_idkelompok)
		{
			var result = _mhsBaruRepo.pengelompokkanMahasiswa(mhs_nopendaftaran, mhs_idkelompok);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/BatalPengelompokkanMahasiswa", Name = "BatalPengelompokkanMahasiswa")]
		public IActionResult BatalPengelompokkanMahasiswa([FromBody] List<string> mhs_nopendaftaran)
		{
			var result = _mhsBaruRepo.batalPengelompokkanMahasiswa(mhs_nopendaftaran);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpGet("/GetMahasiswaByKelompok", Name = "GetMahasiswaByKelompok")]
		public IActionResult GetMahasiswaByKelompok(string kmk_idkelompok)
		{
			try
			{
				List<Dictionary<string, object>> detailList = _mhsBaruRepo.GetMahasiswaByKelompok(kmk_idkelompok);
				return Ok(detailList);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error: {ex.Message}");
			}
		}

		[HttpPut("/verifikasiKelulusanMahasiswa", Name = "verifikasiKelulusanMahasiswa")]
		public IActionResult verifikasiKelulusanMahasiswa([FromBody] List<string> mhs_nopendaftaran)
		{
			var result = _mhsBaruRepo.verifikasiKelulusan(mhs_nopendaftaran);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/batalKelulusanMahasiswa", Name = "batalKelulusanMahasiswa")]
		public IActionResult batalKelulusanMahasiswa([FromBody] List<string> mhs_nopendaftaran)
		{
			var result = _mhsBaruRepo.batalKelulusan(mhs_nopendaftaran);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/ResetKataSandiMahasiswa", Name = "ResetKataSandiMahasiswa")]
		public IActionResult ResetKataSandiMahasiswa(string mhs_nopendaftaran, [FromBody] string mhs_password)
		{
			var result = _mhsBaruRepo.resetPassword(mhs_nopendaftaran, mhs_password);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/UploadSertifikat", Name = "UploadSertifikat")]
		public IActionResult UploadSertifikat(string mhs_nopendaftaran, IFormFile file)
		{
			var result = _mhsBaruRepo.UploadSertifikat(mhs_nopendaftaran, file);
			if (result.status == 200)
			{
				return Ok(result);
			}
			else if (result.status == 404)
			{
				return NotFound(result);
			}
			else
			{
				return StatusCode(500, result);
			}
		}

		[HttpPost("/DownloadSertifikat", Name = "DownloadSertifikat")]
		public IActionResult DownloadSertifikat([FromBody] string fileName)
		{
			var filePath = Path.Combine("C:\\RPL\\PKKMB-API\\Sertifikat", fileName);

			if (System.IO.File.Exists(filePath))
			{
				var fileBytes = System.IO.File.ReadAllBytes(filePath);
				return File(fileBytes, "application/octet-stream", fileName);
			}
			else
			{
				return NotFound();
			}
		}
	}
}
