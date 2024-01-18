using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;
using PKKMB_API.Repository;

namespace PKKMB_API.Controllers
{
	public class NilaiSikapController : Controller
	{
		private readonly NilaiSikapRepository nilaiSikapRepository;
		ResponseModel response = new ResponseModel();

		public NilaiSikapController(IConfiguration configuration)
		{
			nilaiSikapRepository = new NilaiSikapRepository(configuration);
		}

		[HttpGet("/GetAllNilaiSikap", Name = "GetAllNilaiSikap")]
		public IActionResult GetAllNilaiSikap(string nls_idpkkmb)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = nilaiSikapRepository.getAllData(nls_idpkkmb);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed";
			}
			return Ok(response);
		}

		[HttpGet("/GetNilaiSikap", Name = "GetNilaiSikap")]
		public IActionResult GetNilaiSikap(string nls_idnilaisikap)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = nilaiSikapRepository.getData(nls_idnilaisikap);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}

		[HttpPost("/InsertNilaiSikap", Name = "InsertNilaiSikap")]
		public IActionResult InsertNilaiSikap([FromBody] NilaiSikapModel nilaiSikapModel)
		{
			try
			{
				var result = nilaiSikapRepository.insertNilaiSikap(nilaiSikapModel);
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

		[HttpPut("/UpdateNilaiSikap", Name = "UpdateNilaiSikap")]
		public IActionResult UpdateNilaiSikap([FromBody] NilaiSikapModel nilaiSikapModel)
		{
			var result = nilaiSikapRepository.updateNilaiSikap(nilaiSikapModel);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPost("/insertJamPlusMinus", Name = "insertJamPlusMinus")]
		public IActionResult insertJamPlusMinus([FromBody] DetailJamModel jam)
		{
			var result = nilaiSikapRepository.insertJam(jam);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPut("/ubahJamPlusMinus", Name = "ubahJamPlusMinus")]
		public IActionResult ubahJamPlusMinus([FromBody] DetailJamModel jam)
		{
			var result = nilaiSikapRepository.ubahJam(jam);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpGet("/GetDetailJamMahasiswa", Name = "GetDetailJamMahasiswa")]
		public IActionResult GetDetailJamMahasiswa(string dtj_nopendaftaran)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = nilaiSikapRepository.getDetailJamMahasiswa(dtj_nopendaftaran);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}

		[HttpGet("/GetDataDetailJamMahasiswa", Name = "GetDataDetailJamMahasiswa")]
		public IActionResult GetDataDetailJamMahasiswa(string dtj_idjam, string dtj_nopendaftaran)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = nilaiSikapRepository.getDataDetailJamMahasiswa(dtj_idjam, dtj_nopendaftaran);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}
	}
}
