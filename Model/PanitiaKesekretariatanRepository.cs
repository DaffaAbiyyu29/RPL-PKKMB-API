using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class PanitiaKesekretariatanRepository
	{
		private readonly string _connectingString;
		private readonly SqlConnection _connection;
		ResponseModel response = new ResponseModel();

		public PanitiaKesekretariatanRepository(IConfiguration configuration)
		{
			_connectingString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectingString);
		}

		public List<PanitiaKesekretariatanModel> TampilKskAktif()
		{
			List<PanitiaKesekretariatanModel> kskList = new List<PanitiaKesekretariatanModel>();
			try
			{
				string query = "SELECT * FROM view_KskAktif";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel
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
						ksk_idpkkmb = reader["ksk_idpkkmb"].ToString(),
						ksk_status = reader["ksk_status"].ToString(),
					};
					kskList.Add(ksk);
				}
				reader.Close();
				_connection.Close();
				return kskList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public List<PanitiaKesekretariatanModel> TampilFasilitator()
		{
			List<PanitiaKesekretariatanModel> kskList = new List<PanitiaKesekretariatanModel>();
			try
			{
				string query = "SELECT * FROM view_Fasilitator";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel
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
						ksk_idpkkmb = reader["ksk_idpkkmb"].ToString(),
						ksk_status = reader["ksk_status"].ToString(),
					};
					kskList.Add(ksk);
				}
				reader.Close();
				_connection.Close();
				return kskList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public List<PanitiaKesekretariatanModel> TampilKsk()
		{
			List<PanitiaKesekretariatanModel> kskList = new List<PanitiaKesekretariatanModel>();
			try
			{
				string query = "SELECT * FROM view_KSK";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel
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
						ksk_idpkkmb = reader["ksk_idpkkmb"].ToString(),
						ksk_status = reader["ksk_status"].ToString(),
					};
					kskList.Add(ksk);
				}
				reader.Close();
				_connection.Close();
				return kskList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public List<PanitiaKesekretariatanModel> TampilKskDraft()
		{
			List<PanitiaKesekretariatanModel> kskList = new List<PanitiaKesekretariatanModel>();
			try
			{
				string query = "SELECT * FROM view_KskDraft";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel
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
						ksk_idpkkmb = reader["ksk_idpkkmb"].ToString(),
						ksk_status = reader["ksk_status"].ToString(),
					};
					kskList.Add(ksk);
				}
				reader.Close();
				_connection.Close();
				return kskList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public PanitiaKesekretariatanModel getData(string ksk_nim)
		{
			PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel();
			try
			{
				string query = "SELECT * FROM view_KskAktif WHERE ksk_nim = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", ksk_nim);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
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
						ksk_idpkkmb = reader["ksk_idpkkmb"].ToString(),
						ksk_status = reader["ksk_status"].ToString(),
					};

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

		public PanitiaKesekretariatanModel login(string ksk_nim, string password)
		{
			PanitiaKesekretariatanModel ksk = new PanitiaKesekretariatanModel();
			try
			{
				string query = "SELECT * FROM pkm_mskesekretariatan WHERE ksk_nim = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", ksk_nim);
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
								ksk_idpkkmb = reader["ksk_idpkkmb"].ToString(),
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

		public ResponseModel daftarKsk([FromBody] PanitiaKesekretariatanModel ksk)
		{
			try
			{
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(ksk.ksk_password, 12);

				//string query = "INSERT INTO pkm_mskesekretariatan (ksk_nim, ksk_nama, ksk_jeniskelamin, ksk_programstudi, ksk_password, ksk_role, ksk_notelepon, ksk_email, ksk_alamat, ksk_status) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
				SqlCommand command = new SqlCommand("sp_TambahKesekretariatan", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nama", ksk.ksk_nama);
				command.Parameters.AddWithValue("@p_jeniskelamin", ksk.ksk_jeniskelamin);
				command.Parameters.AddWithValue("@p_programstudi", ksk.ksk_programstudi);
				command.Parameters.AddWithValue("@p_password", hashedPassword);
				command.Parameters.AddWithValue("@p_role", ksk.ksk_role);
				command.Parameters.AddWithValue("@p_notelepon", ksk.ksk_notelepon);
				command.Parameters.AddWithValue("@p_email", ksk.ksk_email);
				command.Parameters.AddWithValue("@p_alamat", ksk.ksk_alamat);
				command.Parameters.AddWithValue("@p_idpkkmb", ksk.ksk_idpkkmb);
				command.Parameters.AddWithValue("@p_status", "Tidak Aktif");

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Panitia Kesekretariatan berhasil didaftarkan";
				response.data = ksk;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mendaftarkan Panitia Kesekretariatan = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel updateKsk([FromBody] PanitiaKesekretariatanModel ksk)
		{
			try
			{
				string query = "UPDATE pkm_mskesekretariatan " +
							"SET ksk_nama = @p2, " +
							"ksk_jeniskelamin = @p3, " +
							"ksk_programstudi = @p4, " +
							"ksk_role = @p6, " +
							"ksk_notelepon = @p7, " +
							"ksk_email = @p8, " +
							"ksk_alamat = @p9 " +
							"ksk_idpkkmb = @p10 " +
							"WHERE ksk_nim= @p1";
				using SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", ksk.ksk_nim);
				command.Parameters.AddWithValue("@p2", ksk.ksk_nama);
				command.Parameters.AddWithValue("@p3", ksk.ksk_jeniskelamin);
				command.Parameters.AddWithValue("@p4", ksk.ksk_programstudi);
				command.Parameters.AddWithValue("@p6", ksk.ksk_role);
				command.Parameters.AddWithValue("@p7", ksk.ksk_notelepon);
				command.Parameters.AddWithValue("@p8", ksk.ksk_email);
				command.Parameters.AddWithValue("@p9", ksk.ksk_alamat);
				command.Parameters.AddWithValue("@p10", ksk.ksk_idpkkmb);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Panitia Kesekretariatan berhasil diubah";
				response.data = ksk;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengubah Panitia Kesekretariatan = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel verifikasiKSK([FromBody] List<string> ksk_nim)
		{
			try
			{
				DataTable nimTable = new DataTable();
				nimTable.Columns.Add("Item", typeof(string));

				foreach (string nim in ksk_nim)
				{
					nimTable.Rows.Add(nim);
				}

				SqlCommand command = new SqlCommand("sp_VerifikasiKesekretariatan", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nim", nimTable);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Panitia Kesekretariatan Aktif";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengaktifkan Panitia Kesekretariatan = " + ex.Message;
			}

			return response;
		}

		public ResponseModel nonAktifKsk([FromBody] List<string> ksk_nim)
		{
			try
			{
				DataTable nimTable = new DataTable();
				nimTable.Columns.Add("Item", typeof(string));

				foreach (string nim in ksk_nim)
				{
					nimTable.Rows.Add(nim);
				}

				SqlCommand command = new SqlCommand("sp_NonAktifKesekretariatan", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nim", nimTable);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Panitia Kesekretariatan Aktif";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengaktifkan Panitia Kesekretariatan = " + ex.Message;
			}

			return response;
		}
	}
}
