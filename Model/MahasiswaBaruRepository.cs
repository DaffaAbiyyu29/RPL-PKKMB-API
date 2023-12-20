using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PKKMB_API.Model
{
	public class MahasiswaBaruRepository
	{
		private readonly string _connectingString;
		private readonly SqlConnection _connection;
		ResponseModel response = new ResponseModel();
		private readonly IConfiguration _configuration;

		public MahasiswaBaruRepository(IConfiguration configuration)
		{
			_connectingString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectingString);
			_configuration = configuration;
		}

		public List<MahasiswaBaruModel> getAllData()
		{
			List<MahasiswaBaruModel> mhsList = new List<MahasiswaBaruModel>();
			try
			{
				string query = "SELECT * FROM pkm_msmahasiswa";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					MahasiswaBaruModel mhsBaru = new MahasiswaBaruModel
					{
						mhs_nopendaftaran = reader["mhs_nopendaftaran"].ToString(),
						mhs_namalengkap = reader["mhs_namalengkap"].ToString(),
						mhs_gender = reader["mhs_gender"].ToString(),
						mhs_programstudi = reader["mhs_programstudi"].ToString(),
						mhs_alamat = reader["mhs_alamat"].ToString(),
						mhs_notelepon = reader["mhs_notelepon"].ToString(),
						mhs_email = reader["mhs_email"].ToString(),
						mhs_password = reader["mhs_password"].ToString(),
						mhs_kategori = reader["mhs_kategori"].ToString(),
						mhs_idkelompok = reader["mhs_idkelompok"].ToString(),
						mhs_status = reader["mhs_status"].ToString(),
					};
					mhsList.Add(mhsBaru);
				}
				reader.Close();
				_connection.Close();
				return mhsList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public MahasiswaBaruModel getData(string mhs_nopendaftaran)
		{
			MahasiswaBaruModel mhsBaru = new MahasiswaBaruModel();
			try
			{
				string query = "SELECT * FROM pkm_msmahasiswa WHERE mhs_nopendaftaran = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", mhs_nopendaftaran);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					mhsBaru = new MahasiswaBaruModel
					{
						mhs_nopendaftaran = reader["mhs_nopendaftaran"].ToString(),
						mhs_namalengkap = reader["mhs_namalengkap"].ToString(),
						mhs_gender = reader["mhs_gender"].ToString(),
						mhs_programstudi = reader["mhs_programstudi"].ToString(),
						mhs_alamat = reader["mhs_alamat"].ToString(),
						mhs_notelepon = reader["mhs_notelepon"].ToString(),
						mhs_email = reader["mhs_email"].ToString(),
						mhs_password = reader["mhs_password"].ToString(),
						mhs_kategori = reader["mhs_kategori"].ToString(),
						mhs_idkelompok = reader["mhs_idkelompok"].ToString(),
						mhs_status = reader["mhs_status"].ToString()
					};

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

		public MahasiswaBaruModel login(string mhs_nopendaftaran, string mhs_password)
		{
			MahasiswaBaruModel mhsBaru = new MahasiswaBaruModel();
			try
			{
				string query = "SELECT * FROM pkm_msmahasiswa WHERE mhs_nopendaftaran = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", mhs_nopendaftaran);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					// User found, verify password
					string hashedPasswordFromDb = reader["mhs_password"].ToString();

					if (!string.IsNullOrEmpty(hashedPasswordFromDb))
					{
						bool verify = BCrypt.Net.BCrypt.Verify(mhs_password, hashedPasswordFromDb);

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
								mhs_password = reader["mhs_password"].ToString(),
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

		public string CreateToken(MahasiswaBaruModel mhs)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!);
			var tokenDeskriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("mhs_nopendaftaran", mhs.mhs_nopendaftaran),
					new Claim(JwtRegisteredClaimNames.Name, mhs.mhs_namalengkap),
					new Claim(JwtRegisteredClaimNames.Email, mhs.mhs_email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				}),
				Expires = DateTime.UtcNow.AddMinutes(2),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
			};

			var token = jwtTokenHandler.CreateToken(tokenDeskriptor);
			var jwtToken = jwtTokenHandler.WriteToken(token);

			return jwtToken;
		}

		public ResponseModel daftarMahasiswa([FromBody] MahasiswaBaruModel mhsBaru)
		{
			try
			{
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(mhsBaru.mhs_password, 12);

				//string query = "INSERT INTO pkm_msmahasiswa (mhs_nopendaftaran, mhs_namalengkap, mhs_gender, mhs_programstudi, mhs_alamat, mhs_notelepon, mhs_email, mhs_password, mhs_kategori, mhs_status) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
				SqlCommand command = new SqlCommand("sp_TambahMahasiswa", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_namalengkap", mhsBaru.mhs_namalengkap);
				command.Parameters.AddWithValue("@p_gender", mhsBaru.mhs_gender);
				command.Parameters.AddWithValue("@p_programstudi", mhsBaru.mhs_programstudi);
				command.Parameters.AddWithValue("@p_alamat", mhsBaru.mhs_alamat);
				command.Parameters.AddWithValue("@p_notelepon", mhsBaru.mhs_notelepon);
				command.Parameters.AddWithValue("@p_email", mhsBaru.mhs_email);
				command.Parameters.AddWithValue("@p_password", hashedPassword);
				command.Parameters.AddWithValue("@p_kategori", mhsBaru.mhs_kategori);
				command.Parameters.AddWithValue("@p_statuskelulusan", "-");
				command.Parameters.AddWithValue("@p_status", "Menunggu Verifikasi");

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Mendaftar";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = ex.Message;
			}

			return response;
		}

		public ResponseModel updateMahasiswa([FromBody] MahasiswaBaruModel mhsBaru)
		{
			try
			{
				//string hashedPassword = BCrypt.Net.BCrypt.HashPassword(mhsBaru.mhs_password, 12);

				string query = "UPDATE pkm_msmahasiswa " +
							"SET mhs_namalengkap = @p2, " +
							"mhs_programstudi = @p3, " +
							"mhs_alamat = @p4, " +
							"mhs_notelepon = @p5, " +
							"mhs_email = @p6, " +
							"mhs_kategori = @p7 " +
							"WHERE mhs_nopendaftaran= @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				/*command.CommandType = System.Data.CommandType.StoredProcedure;*/
				command.Parameters.AddWithValue("@p1", mhsBaru.mhs_nopendaftaran);
				command.Parameters.AddWithValue("@p2", mhsBaru.mhs_namalengkap);
				command.Parameters.AddWithValue("@p3", mhsBaru.mhs_programstudi);
				command.Parameters.AddWithValue("@p4", mhsBaru.mhs_alamat);
				command.Parameters.AddWithValue("@p5", mhsBaru.mhs_notelepon);
				command.Parameters.AddWithValue("@p6", mhsBaru.mhs_email);
				command.Parameters.AddWithValue("@p7", mhsBaru.mhs_kategori);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Mahasiswa baru berhasil diubah";
				response.data = mhsBaru;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengubah mahasiswa baru = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel deleteMahasiswa([FromBody] string mhs_nopendaftaran)
		{
			try
			{
				string query = "delete from pkm_msmahasiswa where mhs_nopendaftaran= @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", mhs_nopendaftaran);
				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Mahasiswa baru berhasil dihapus";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat menghapus mahasiswa baru = " + ex.Message;
			}

			return response;
		}
	}
}
