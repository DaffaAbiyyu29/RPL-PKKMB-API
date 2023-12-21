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
	}
}
