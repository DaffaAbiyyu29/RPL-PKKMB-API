using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PicPkkmbController : Controller
	{
		private readonly PicPkkmbRepository _picRepo;
		private readonly IConfiguration _configuration;

		public PicPkkmbController(IConfiguration configuration)
		{
			_picRepo = new PicPkkmbRepository(configuration);
			_configuration = configuration;
		}

		[HttpGet("/getallpic", Name = "GetAllPic")]
		public IActionResult GetAllPic()
		{
			var pic = _picRepo.getAllData();
			try
			{
				if (pic != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data PIC", Data = pic });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data PIC Tidak Ditemukan", Data = pic });
				}
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data PIC", Data = ex.Message });
			}
		}

		[HttpGet("/datapic", Name = "DataPic")]
		public IActionResult DataPic(string pic_npk)
		{
			PicPkkmbModel pic = _picRepo.getData(pic_npk);
			try
			{
				if (pic != null)
				{
					return Ok(new { Status = 200, Messages = "Akun ditemukan", Data = pic });

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

		[HttpGet("/loginpic", Name = "loginPic")]
		public IActionResult loginPic(string pic_npk, string pic_password)
		{
			PicPkkmbModel pic = _picRepo.login(pic_npk, pic_password);
			try
			{
				if (pic != null)
				{
					if (pic.pic_password != null)
					{
						// Password is correct, login successful
						/*HttpContext.Session.SetString("Peran", "Berhasil");*/
						return Ok(new { Status = 200, Messages = "Login berhasil", Data = pic });
					}
					else
					{
						// Password is incorrect
						return Unauthorized(new { Status = 401, Messages = "Kata Sandi Salah", Data = new Object() });
					}
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

		[HttpPost("/daftarpic", Name = "DaftarPic")]
		public IActionResult DaftarPic([FromBody] PicPkkmbModel pic)
		{
			var result = _picRepo.daftarPIC(pic);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages, Data = result.data });
		}

		[HttpPut("/ubahpic", Name = "UpdatePic")]
		public IActionResult UpdatePic([FromBody] PicPkkmbModel pic)
		{
			var result = _picRepo.updatePIC(pic);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages, Data = result.data });
		}

		[HttpDelete("/hapuspic", Name = "HapusPic")]
		public IActionResult HapusPic(string pic_npk)
		{
			var result = _picRepo.deletePIC(pic_npk);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}

		[HttpPost("/createinformasipkkmb", Name = "CreateInformasi")]
		public IActionResult CreateInformasi(string pic_npk)
		{
			var result = _picRepo.deletePIC(pic_npk);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}
	}
}
