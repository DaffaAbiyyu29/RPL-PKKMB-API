using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;
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
		public IActionResult GetAllMahasiswa()
		{
			var role = HttpContext.Request.Cookies["role"];
			try
			{
				/*if (role == "Mahasiswa")
				{*/
				var mhsBaru = _mhsBaruRepo.getAllData();
				if (mhsBaru != null)
				{
					return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data Mahasiswa", Data = mhsBaru });
				}
				else
				{
					return StatusCode(404, new { Status = 404, Messages = "Data Mahasiswa Tidak Ditemukan", Data = mhsBaru });
				}
				/*}
				else
				{
					return Unauthorized(new { Status = 401, Messages = "Unauthorized", Data = new Object() });
				}*/
			}
			catch (Exception ex)
			{
				// Tangani kesalahan umum
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

		[HttpGet("/loginmahasiswa", Name = "loginMahasiswa")]
		//[AllowAnonymous]
		public IActionResult loginMahasiswa(string mhs_nopendaftaran, string mhs_password)
		{
			MahasiswaBaruModel mhsBaru = _mhsBaruRepo.login(mhs_nopendaftaran, mhs_password);
			try
			{
				if (mhsBaru != null)
				{
					if (mhsBaru.mhs_password != null)
					{
						// Password is correct, login successful
						var token = _mhsBaruRepo.CreateToken(mhsBaru);

						// Set JWT token in cookies
						Response.Cookies.Append("JWTToken", token, new CookieOptions
						{
							HttpOnly = true,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						Response.Cookies.Append("Role", "Mahasiswa", new CookieOptions
						{
							HttpOnly = false,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						return Ok(new { Status = 200, Messages = "Login berhasil", Data = mhsBaru, Token = token });
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

		[HttpDelete("/hapusmahasiswa", Name = "HapusMahasiswaBaru")]
		public IActionResult HapusMahasiswaBaru([FromBody] string mhs_nopendaftaran)
		{
			var result = _mhsBaruRepo.deleteMahasiswa(mhs_nopendaftaran);
			return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
		}
	}
}
