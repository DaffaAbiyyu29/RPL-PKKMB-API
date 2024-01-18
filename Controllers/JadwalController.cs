using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	public class JadwalController : Controller
	{
		private readonly JadwalRepository jadwalRepository;
		ResponseModel response = new ResponseModel();

		public JadwalController(IConfiguration configuration)
		{
			jadwalRepository = new JadwalRepository(configuration);
		}

		[HttpGet("/GetAllJadwal", Name = "GetAllJadwal")]
		public IActionResult GetAllJadwal(string jdl_idpkkmb)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = jadwalRepository.getAllData(jdl_idpkkmb);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed";
			}
			return Ok(response);
		}

		[HttpGet("/GetJadwal", Name = "GetJadwal")]
		public IActionResult GetJadwal(string jdl_idjadwal)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = jadwalRepository.getData(jdl_idjadwal);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}

		[HttpPost("/InsertJadwal", Name = "InsertJadwal")]
		public IActionResult InsertJadwal([FromBody] JadwalModel jadwalModel)
		{
			try
			{
				var result = jadwalRepository.insertJadwal(jadwalModel);
				response.status = 200;
				response.messages = "Success";
				response.data = result;
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;

			}
			return Ok(response);
		}

		[HttpPut("/UbahJadwal", Name = "UbahJadwal")]
		public IActionResult UpdateJadwal([FromBody] JadwalModel jadwalModel)
		{
			JadwalModel jadwal = new JadwalModel
			{
				jdl_idjadwal = jadwalModel.jdl_idjadwal,
				jdl_nim = jadwalModel.jdl_nim,
				jdl_tglpelaksanaan = jadwalModel.jdl_tglpelaksanaan,
				jdl_waktupelaksanaan = jadwalModel.jdl_waktupelaksanaan,
				jdl_agenda = jadwalModel.jdl_agenda,
				jdl_tempat = jadwalModel.jdl_tempat,
				jdl_idpkkmb = jadwalModel.jdl_idpkkmb,
				jdl_status = jadwalModel.jdl_status
			};

			try
			{
				var result = jadwalRepository.UpdateJadwal(jadwal); // Assuming UpdateJadwal method calls the stored procedure
				response.status = 200;
				response.messages = "Success";
				response.data = result;
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
