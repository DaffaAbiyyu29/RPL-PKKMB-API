using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PKKMB_API.Model;
using PKKMB_API.Repository;
using System;

namespace PKKMB_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TugasController : ControllerBase
	{
		private readonly TugasRepository tugasRepository;
		private readonly ResponseModel response = new ResponseModel();

		public TugasController(IConfiguration configuration)
		{
			tugasRepository = new TugasRepository(configuration);
		}

		[HttpGet("/NextId", Name = "NextId")]
		public IActionResult NextId()
		{
			var role = HttpContext.Request.Cookies["role"];
			try
			{
				/*if (role == "Mahasiswa")
				{*/
				var tugasList = tugasRepository.NextId();
				if (tugasList != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Tugas", Data = tugasList });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data Tugas Tidak Ditemukan", Data = tugasList });
				}
				/*}
				else
				{
					return Unauthorized(new { Status = 401, Messages = "Unauthorized", Data = new Object() });
				}*/
			}
			catch (Exception ex)
			{
				// Handle common errors
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Tugas", Data = ex.Message });
			}
		}

		[HttpGet("/getalltugas", Name = "GetAllTugas")]
		public IActionResult GetAllTugas()
		{
			var role = HttpContext.Request.Cookies["role"];
			try
			{
				/*if (role == "Mahasiswa")
				{*/
				var tugasList = tugasRepository.GetAllData();
				if (tugasList != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Tugas", Data = tugasList });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data Tugas Tidak Ditemukan", Data = tugasList });
				}
				/*}
				else
				{
					return Unauthorized(new { Status = 401, Messages = "Unauthorized", Data = new Object() });
				}*/
			}
			catch (Exception ex)
			{
				// Handle common errors
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Tugas", Data = ex.Message });
			}
		}

		[HttpGet("/datatugas", Name = "DataTugas")]
		//[Authorize(Roles = "Mahasiswa")]
		public IActionResult DataTugas(string tgs_idtugas)
		{
			TugasModel tugas = tugasRepository.GetData(tgs_idtugas);
			try
			{
				if (tugas != null)
				{
					return Ok(new { Status = 200, Messages = "Data Tugas ditemukan", Data = tugas });
				}
				else
				{
					// Data not found
					return NotFound(new { Status = 404, Messages = "Data Tugas Tidak Ditemukan", Data = new Object() });
				}
			}
			catch (Exception ex)
			{
				// General error
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Mengambil Data Tugas = " + ex.Message, Data = new Object() });
			}
		}

		[HttpPost("/InsertTugas", Name = "InsertTugas")]
		public IActionResult InsertTugas([FromBody] TugasModel tugasModel)
		{
			try
			{
				var result = tugasRepository.InsertTugas(tugasModel);
				response.status = 200;
				response.messages = "Success";
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}

		[HttpPut("/UpdateTugas", Name = "UpdateTugas")]
		public IActionResult UpdateTugas([FromBody] TugasModel tugasModel)
		{
			try
			{
				var result = tugasRepository.UpdateTugas(tugasModel);
				response.status = 200;
				response.messages = "Success";
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex.Message;
			}
			return Ok(response);
		}

		[HttpPost("Upload")]
		public IActionResult UploadFile(string tgs_nim, string tgs_namatugas, DateTime tgs_tglpemberiantugas, IFormFile file, DateTime tgs_deadline, string tgs_deskripsi)
		{
			var result = tugasRepository.UploadFile(tgs_nim, tgs_namatugas, tgs_tglpemberiantugas, file, tgs_deadline, tgs_deskripsi);
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

		[HttpPut("Ubah")]
		public IActionResult UbahFile(string tgs_idtugas, string tgs_nim, string tgs_jenistugas, DateTime tgs_tglpemberiantugas, IFormFile file, DateTime tgs_deadline, string tgs_deskripsi, string tgs_status)
		{
			var result = tugasRepository.UbahFile(tgs_idtugas, tgs_nim, tgs_jenistugas, tgs_tglpemberiantugas, file, tgs_deadline, tgs_deskripsi, tgs_status);
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

		[HttpPost("Download")]
		public IActionResult DownloadFile([FromBody] string fileName)
		{
			var filePath = Path.Combine("C:\\RPL\\PKKMB-API\\File_Tugas\\TugasMahasiswa", fileName);

			if (System.IO.File.Exists(filePath))
			{
				var fileBytes = System.IO.File.ReadAllBytes(filePath);
				return File(fileBytes, "application/octet-stream", fileName);
			}
			else
			{
				return NotFound(); // File not found response
			}
		}

		[HttpPost("DownloadTugasMahasiswa")]
		public IActionResult DownloadTugasMahasiswa([FromBody] string fileName)
		{
			var filePath = Path.Combine("C:\\RPL\\PKKMB-API\\File_Tugas\\PengumpulanTugas", fileName);

			if (System.IO.File.Exists(filePath))
			{
				var fileBytes = System.IO.File.ReadAllBytes(filePath);
				return File(fileBytes, "application/octet-stream", fileName);
			}
			else
			{
				return NotFound(); // File not found response
			}
		}

		[HttpGet("GetAllDetail", Name = "GetAllDetail")]
		public IActionResult GetAllDetail()
		{
			var role = HttpContext.Request.Cookies["role"];
			try
			{
				/*if (role == "Mahasiswa")
				{*/
				var detailList = tugasRepository.GetAllDataDetail();
				if (detailList != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Detail Tugas", Data = detailList });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data Detail Tugas Tidak Ditemukan", Data = detailList });
				}
				/*}
				else
				{
					return Unauthorized(new { Status = 401, Messages = "Unauthorized", Data = new Object() });
				}*/
			}
			catch (Exception ex)
			{
				// Handle common errors
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data Detail Tugas", Data = ex.Message });
			}
		}

		[HttpGet("DataDetailTugas", Name = "DataDetailTugas")]
		//[Authorize(Roles = "Mahasiswa")]
		public IActionResult DataDetailTugas(string dts_iddetail, string dts_nopendaftaran)
		{
			DetailTugasModel detail = tugasRepository.GetDataDetail(dts_iddetail, dts_nopendaftaran);
			try
			{
				if (detail != null)
				{
					return Ok(new { Status = 200, Messages = "Data Detail Tugas ditemukan", Data = detail });
				}
				else
				{
					// Data not found
					return NotFound(new { Status = 404, Messages = "Data Detail Tugas Tidak Ditemukan", Data = new Object() });
				}
			}
			catch (Exception ex)
			{
				// General error
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Mengambil Data Detail Tugas = " + ex.Message, Data = new Object() });
			}
		}

		[HttpPost("UploadTugas")]
		public IActionResult UploadTugas(string dts_iddetail, string dts_nopendaftaran, IFormFile file, DateTime dts_waktupengumpulan, double dts_nilaitugas)
		{
			var result = tugasRepository.UploadTugasMahasiswa(dts_iddetail, dts_nopendaftaran, file, dts_waktupengumpulan, dts_nilaitugas);
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

		[HttpPut("UbahTugas")]
		public IActionResult UbahTugas(string dts_iddetail, string dts_nopendaftaran, IFormFile file, DateTime dts_waktupengumpulan, double dts_nilaitugas)
		{
			var result = tugasRepository.UbahTugasMahasiswa(dts_iddetail, dts_nopendaftaran, file, dts_waktupengumpulan, dts_nilaitugas);
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

		[HttpGet("GetDetailTugasByKelompok")]
		public IActionResult GetDetailTugasByKelompok(string id_tugas, string id_kelompok)
		{
			try
			{
				List<Dictionary<string, object>> detailList = tugasRepository.GetDetailTugasByKelompok(id_tugas, id_kelompok);
				return Ok(detailList);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error: {ex.Message}");
			}
		}

		[HttpPut("PenilaianTugas")]
		public IActionResult PenilaianTugas(string dts_iddetail, string dts_nopendaftaran, [FromBody] double dts_nilaitugas)
		{
			var result = tugasRepository.PenilaianTugas(dts_iddetail, dts_nopendaftaran, dts_nilaitugas);
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
	}
}
