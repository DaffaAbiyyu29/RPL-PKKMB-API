using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PKKMB_API.Model
{
	public class LoginRepository
	{
		private readonly string _connectingString;
		private readonly SqlConnection _connection;
		private readonly IConfiguration _configuration;

		public LoginRepository(IConfiguration configuration)
		{
			_connectingString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectingString);
			_configuration = configuration;
		}

		public MahasiswaBaruModel loginMahasiswa([FromBody] string username, string password)
		{
			MahasiswaBaruModel mhsBaru = new MahasiswaBaruModel();
			try
			{
				string query = "SELECT * FROM pkm_msmahasiswa WHERE mhs_nopendaftaran = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", username);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					// User found, verify password
					string hashedPasswordFromDb = reader["mhs_password"].ToString();

					if (!string.IsNullOrEmpty(hashedPasswordFromDb))
					{
						bool verify = BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb);

						if (verify)
						{
							// Password is correct, populate MahasiswaBaruModel
							mhsBaru = new MahasiswaBaruModel
							{
								mhs_nopendaftaran = reader["mhs_nopendaftaran"].ToString(),
								mhs_namalengkap = reader["mhs_namalengkap"].ToString(),
								mhs_gender = reader["mhs_gender"].ToString(),
								mhs_programstudi = reader["mhs_programstudi"].ToString(),
								mhs_alamat = reader["mhs_alamat"].ToString(),
								mhs_notelepon = reader["mhs_notelepon"].ToString(),
								mhs_email = reader["mhs_email"].ToString(),
								mhs_password = hashedPasswordFromDb,
								mhs_kategori = reader["mhs_kategori"].ToString(),
								mhs_idkelompok = reader["mhs_idkelompok"].ToString(),
								mhs_status = reader["mhs_status"].ToString()
							};
						}
					}

					reader.Close();
					_connection.Close();
					return mhsBaru;
				}
				else
				{
					// User not found
					reader.Close();
					_connection.Close();
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public PanitiaKesekretariatanModel loginKSK([FromBody] string username, string password)
		{
			PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel();
			try
			{
				string query = "SELECT * FROM pkm_mskesekretariatan WHERE ksk_nim = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", username);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					// User found, verify password
					string hashedPasswordFromDb = reader["ksk_password"].ToString();

					if (!string.IsNullOrEmpty(hashedPasswordFromDb))
					{
						bool verify = BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb);

						if (verify)
						{
							// Password is correct, populate PanitiaKesekretariatanModel
							ksk = new PanitiaKesekretariatanModel
							{
								ksk_nim = reader["ksk_nim"].ToString(),
								ksk_nama = reader["ksk_nama"].ToString(),
								ksk_jeniskelamin = reader["ksk_jeniskelamin"].ToString(),
								ksk_programstudi = reader["ksk_programstudi"].ToString(),
								ksk_password = reader["ksk_password"].ToString(),
								ksk_role = reader["ksk_role"].ToString(),
								ksk_notelepon = reader["ksk_notelepon"].ToString(),
								ksk_email = reader["ksk_email"].ToString(),
								ksk_alamat = reader["ksk_alamat"].ToString(),
								ksk_status = reader["ksk_status"].ToString(),
							};
						}
					}

					reader.Close();
					_connection.Close();
					return ksk;
				}
				else
				{
					// User not found
					reader.Close();
					_connection.Close();
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public PicPkkmbModel loginPIC([FromBody] string username, string password)
		{
			PicPkkmbModel pic = new PicPkkmbModel();
			try
			{
				string query = "SELECT * FROM pkm_mspicpkkmb WHERE pic_nokaryawan = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", username);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					// User found, verify password
					string hashedPasswordFromDb = reader["pic_password"].ToString();

					if (!string.IsNullOrEmpty(hashedPasswordFromDb))
					{
						bool verify = BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb);

						if (verify)
						{
							// Password is correct, populate PicPkkmbModel
							pic = new PicPkkmbModel
							{
								pic_nokaryawan = reader["pic_nokaryawan"].ToString(),
								pic_nama = reader["pic_nama"].ToString(),
								pic_password = reader["pic_password"].ToString(),
								pic_status = reader["pic_status"].ToString(),
							};
						}
					}

					reader.Close();
					_connection.Close();
					return pic;
				}
				else
				{
					// User not found
					reader.Close();
					_connection.Close();
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public string GenerateJwtToken<T>(T user)
		{
			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:SecretKey").Value);
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(GetUserClaims(user)),
				Expires = DateTime.UtcNow.AddMinutes(10), // Set the expiration time as needed
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			var jwtToken = jwtTokenHandler.WriteToken(token);

			return jwtToken;
		}

		private Claim[] GetUserClaims<T>(T user)
		{
			if (user is MahasiswaBaruModel mhs)
			{
				return new[]
				{
					new Claim("id", mhs.mhs_nopendaftaran),
					new Claim(JwtRegisteredClaimNames.Name, mhs.mhs_namalengkap),
					new Claim(ClaimTypes.Role, "Mahasiswa"),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					// Add more claims specific to MahasiswaBaruModel if needed
				};
			}
			else if (user is PanitiaKesekretariatanModel ksk)
			{
				return new[]
				{
					new Claim("id", ksk.ksk_nim),
					new Claim(JwtRegisteredClaimNames.Name, ksk.ksk_nama),
					new Claim(ClaimTypes.Role, ksk.ksk_role),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					// Add more claims specific to PanitiaKesekretariatanModel if needed
				};
			}
			else if (user is PicPkkmbModel pic)
			{
				return new[]
				{
					new Claim("id", pic.pic_nokaryawan),
					new Claim(JwtRegisteredClaimNames.Name, pic.pic_nama),
					new Claim(ClaimTypes.Role, "PIC PKKMB"),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					// Add more claims specific to PicPkkmbModel if needed
				};
			}

			return Array.Empty<Claim>();
		}

		public bool ValidateJwtToken(string jwtToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:SecretKey").Value);

			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false, // Set to true if you want to validate the issuer
				ValidateAudience = false, // Set to true if you want to validate the audience
				ValidateLifetime = true, // Set to true if you want to validate the expiration time
				ClockSkew = TimeSpan.Zero // Set the clock skew to zero to account for time differences
			};

			SecurityToken validatedToken;
			try
			{
				ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
				// You can access the claims from the principal variable if needed
				// e.g., var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				return true; // Token is valid
			}
			catch (Exception ex)
			{
				// Token validation failed
				return false;
			}
		}
	}
}