using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
						mhs_idpkkmb = reader["mhs_idpkkmb"].ToString(),
						mhs_statuskelulusan = reader["mhs_statuskelulusan"].ToString(),
						mhs_status = reader["mhs_status"].ToString(),
						mhs_saran = reader["mhs_saran"].ToString(),
						mhs_kritik = reader["mhs_kritik"].ToString(),
						mhs_insight = reader["mhs_insight"].ToString(),
						mhs_tglkirimevaluasi = DateTime.TryParse(reader["mhs_tglkirimevaluasi"].ToString(), out DateTime tglkirimevaluasi)
								? tglkirimevaluasi
								: DateTime.MinValue,
						mhs_jamplus = int.Parse(reader["mhs_jamplus"].ToString()),
						mhs_jamminus = int.Parse(reader["mhs_jamminus"].ToString()),
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
						mhs_idpkkmb = reader["mhs_idpkkmb"].ToString(),
						mhs_statuskelulusan = reader["mhs_statuskelulusan"].ToString(),
						mhs_status = reader["mhs_status"].ToString(),
						mhs_saran = reader["mhs_saran"].ToString(),
						mhs_kritik = reader["mhs_kritik"].ToString(),
						mhs_insight = reader["mhs_insight"].ToString(),
						mhs_tglkirimevaluasi = DateTime.TryParse(reader["mhs_tglkirimevaluasi"].ToString(), out DateTime tglkirimevaluasi)
								? tglkirimevaluasi
								: DateTime.MinValue,
						mhs_jamplus = int.Parse(reader["mhs_jamplus"].ToString()),
						mhs_jamminus = int.Parse(reader["mhs_jamminus"].ToString()),
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
								mhs_idpkkmb = reader["mhs_idpkkmb"].ToString(),
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
				command.Parameters.AddWithValue("@p_status", "Aktif");
				command.Parameters.AddWithValue("@p_idpkkmb", mhsBaru.mhs_idpkkmb);

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
				SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", mhsBaru.mhs_nopendaftaran);
				command.Parameters.AddWithValue("@p_namalengkap", mhsBaru.mhs_namalengkap);
				command.Parameters.AddWithValue("@p_gender", mhsBaru.mhs_gender);
				command.Parameters.AddWithValue("@p_programstudi", mhsBaru.mhs_programstudi);
				command.Parameters.AddWithValue("@p_alamat", mhsBaru.mhs_alamat);
				command.Parameters.AddWithValue("@p_notelepon", mhsBaru.mhs_notelepon);
				command.Parameters.AddWithValue("@p_email", mhsBaru.mhs_email);
				command.Parameters.AddWithValue("@p_kategori", mhsBaru.mhs_kategori);
				command.Parameters.AddWithValue("@p_idpkkmb", mhsBaru.mhs_idpkkmb);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Mahasiswa berhasil diubah";
				response.data = mhsBaru;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengubah Mahasiswa = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel statusKelulusan([FromBody] string nopendaftaran)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_KelulusanMahasiswa", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", nopendaftaran);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Mengubah Status Kelulusan";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi Kesalahan Saat Mengubah Status Kelulusan = " + ex.Message;
			}

			return response;
		}

		public ResponseModel evaluasiMahasiswa([FromBody] MahasiswaBaruModel mhsBaru)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_EvaluasiMahasiswa", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", mhsBaru.mhs_nopendaftaran);
				command.Parameters.AddWithValue("@p_kritik", mhsBaru.mhs_kritik);
				command.Parameters.AddWithValue("@p_saran", mhsBaru.mhs_saran);
				command.Parameters.AddWithValue("@p_umpanbalik", mhsBaru.mhs_insight);
				command.Parameters.AddWithValue("@p_tglkirimevaluasi", mhsBaru.mhs_tglkirimevaluasi);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Mengirim Evaluasi";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi Kesalahan Saat Mengirim Evaluasi = " + ex.Message;
			}

			return response;
		}

		public List<MahasiswaBaruModel> TampilMahasiswaTanpaKelompok()
		{
			List<MahasiswaBaruModel> mhsList = new List<MahasiswaBaruModel>();
			try
			{
				string query = "SELECT * FROM view_MahasiswaTanpaKelompok";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					MahasiswaBaruModel mhs = new MahasiswaBaruModel
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
						mhs_idpkkmb = reader["mhs_idpkkmb"].ToString(),
						mhs_statuskelulusan = reader["mhs_statuskelulusan"].ToString(),
						mhs_status = reader["mhs_status"].ToString(),
						mhs_saran = reader["mhs_saran"].ToString(),
						mhs_kritik = reader["mhs_kritik"].ToString(),
						mhs_insight = reader["mhs_insight"].ToString(),
						mhs_tglkirimevaluasi = DateTime.TryParse(reader["mhs_tglkirimevaluasi"].ToString(), out DateTime tglkirimevaluasi)
								? tglkirimevaluasi
								: DateTime.MinValue
					};
					mhsList.Add(mhs);
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

		public ResponseModel pengelompokkanMahasiswa([FromBody] List<string> mhs_nopendaftaran, string mhs_idkelompok)
		{
			try
			{
				DataTable idTable = new DataTable();
				idTable.Columns.Add("Item", typeof(string));

				foreach (string nopendaftaran in mhs_nopendaftaran)
				{
					idTable.Rows.Add(nopendaftaran);
				}

				SqlCommand command = new SqlCommand("sp_PengelompokanMahasiswa", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", idTable);
				command.Parameters.AddWithValue("@p_idkelompok", mhs_idkelompok);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Mengelompokkan Mahasiswa";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi Kesalahan Saat Mengelompokkan Mahasiswa = " + ex.Message;
			}

			return response;
		}

		public ResponseModel batalPengelompokkanMahasiswa([FromBody] List<string> mhs_nopendaftaran)
		{
			try
			{
				DataTable idTable = new DataTable();
				idTable.Columns.Add("Item", typeof(string));

				foreach (string nopendaftaran in mhs_nopendaftaran)
				{
					idTable.Rows.Add(nopendaftaran);
				}

				SqlCommand command = new SqlCommand("sp_BatalPengelompokanMahasiswa", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", idTable);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Membatalkan Pengelompokkan Mahasiswa";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi Kesalahan Saat Membatalkan Pengelompokkan Mahasiswa = " + ex.Message;
			}

			return response;
		}

		public List<Dictionary<string, object>> GetMahasiswaByKelompok(string kelompokId)
		{
			List<Dictionary<string, object>> detailList = new List<Dictionary<string, object>>();
			try
			{
				string query = "SELECT * FROM pkm_msmahasiswa WHERE mhs_status = 'Aktif' AND mhs_idkelompok = @KelompokId";

				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@KelompokId", kelompokId);

				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Dictionary<string, object> detail = new Dictionary<string, object>
					{
						{ "mhs_nopendaftaran", reader["mhs_nopendaftaran"].ToString() },
						{ "mhs_namalengkap", reader["mhs_namalengkap"].ToString() },
						{ "mhs_gender", reader["mhs_gender"].ToString() },
						{ "mhs_programstudi", reader["mhs_programstudi"].ToString() },
						{ "mhs_kategori", reader["mhs_kategori"].ToString() },
						{ "mhs_idkelompok", reader["mhs_idkelompok"].ToString() },
						{ "mhs_idpkkmb", reader["mhs_idpkkmb"].ToString() },
					};
					detailList.Add(detail);
				}

				reader.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				_connection.Close();
			}

			return detailList;
		}

		public ResponseModel verifikasiKelulusan([FromBody] List<string> mhs_nopendaftaran)
		{
			try
			{
				DataTable nimTable = new DataTable();
				nimTable.Columns.Add("Item", typeof(string));

				foreach (string nim in mhs_nopendaftaran)
				{
					nimTable.Rows.Add(nim);
				}

				SqlCommand command = new SqlCommand("sp_VerifikasiKelulusanMhs", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", nimTable);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Meluluskan Mahasiswa Baru";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat Meluluskan Mahasiswa Baru = " + ex.Message;
			}

			return response;
		}

		public ResponseModel batalKelulusan([FromBody] List<string> mhs_nopendaftaran)
		{
			try
			{
				DataTable nimTable = new DataTable();
				nimTable.Columns.Add("Item", typeof(string));

				foreach (string nim in mhs_nopendaftaran)
				{
					nimTable.Rows.Add(nim);
				}

				SqlCommand command = new SqlCommand("sp_batalKelulusanMhs", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", nimTable);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Berhasil Membatalkan Keluluskan Mahasiswa Baru";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat Membatalkan Keluluskan Mahasiswa Baru = " + ex.Message;
			}

			return response;
		}
	}
}