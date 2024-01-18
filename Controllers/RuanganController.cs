using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	public class RuanganController : Controller
	{
		private readonly RuanganRepository ruanganRepository;
		ResponseModel response = new ResponseModel();

		public RuanganController(IConfiguration configuration)
		{
			ruanganRepository = new RuanganRepository(configuration);
		}

		[HttpGet("/GetAllRuangan", Name = "GetAllRuangan")]
		public IActionResult GetAllRuangan(string rng_idpkkmb)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = ruanganRepository.getAllData(rng_idpkkmb);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed";
			}
			return Ok(response);
		}

		[HttpGet("/GetRuangan", Name = "GetRuangan")]
		public IActionResult GetRuangan(string rng_idruangan)
		{
			try
			{
				response.status = 200;
				response.messages = "Success";
				response.data = ruanganRepository.getData(rng_idruangan);
			}
			catch (Exception ex)
			{
				response.status = 500;
				response.messages = "Failed, " + ex;
			}
			return Ok(response);
		}

		[HttpPost("/InsertRuangan", Name = "InsertRuangan")]
		public IActionResult InsertRuangan([FromBody] RuanganModel ruanganModel)
		{

			try
			{
				var result = ruanganRepository.insertRuangan(ruanganModel);
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

		[HttpPut("/UpdateRuangan", Name = "UpdateRuangan")]
		public IActionResult UpdateRuangan([FromBody] RuanganModel ruanganModel)
		{
			RuanganModel ruangan = new RuanganModel();
			ruangan.rng_idruangan = ruanganModel.rng_idruangan;
			ruangan.rng_namaruangan = ruanganModel.rng_namaruangan;
			ruangan.rng_idpkkmb = ruanganModel.rng_idpkkmb;
			ruangan.rng_status = ruanganModel.rng_status;
			try
			{
				var result = ruanganRepository.updateRuangan(ruangan);
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
