using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	public class InformasiController : Controller
	{
		private readonly InformasiRepository informasiRepository;
		ResponseModel response = new ResponseModel();

		public InformasiController(IConfiguration configuration)
		{
			informasiRepository = new InformasiRepository(configuration);
		}

		[HttpGet("/GetAllInformasi", Name = "GetAllInformasi")]
		public IActionResult GetAllInformasi(string inf_idpkkmb)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = informasiRepository.getAllData(inf_idpkkmb);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed";
			}
			return Ok(response);
		}

		[HttpGet("/GetInformasi", Name = "GetInformasi")]
		public IActionResult GetInformasi(string inf_idinformasi)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = informasiRepository.GetData(inf_idinformasi);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}

		[HttpPost("/InsertInformasi", Name = "InsertInformasi")]
		public IActionResult InsertInformasi([FromBody] InformasiModel informasiModel)
		{
			try
			{
				var result = informasiRepository.InsertInformasi(informasiModel);
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

		[HttpPut("/UpdateInformasi", Name = "UpdateInformasi")]
		public IActionResult UpdateInformasi([FromBody] InformasiModel informasiModel)
		{
			try
			{
				var result = informasiRepository.UpdateInformasi(informasiModel);
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

		public IActionResult Index()
		{
			return View();
		}

	}
}
