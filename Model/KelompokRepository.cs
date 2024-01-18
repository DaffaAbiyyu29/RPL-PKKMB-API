using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class KelompokRepository
	{
		private readonly string _connectingString;
		private readonly SqlConnection _connection;
		ResponseModel response = new ResponseModel();

		public KelompokRepository(IConfiguration configuration)
		{
			_connectingString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectingString);
		}

		public List<KelompokModel> TampilKelompok(string kmk_idpkkmb)
		{
			List<KelompokModel> kelList = new List<KelompokModel>();
			try
			{
				string query = "SELECT * FROM pkm_mskelompok WHERE kmk_idpkkmb = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", kmk_idpkkmb);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					KelompokModel kel = new KelompokModel
					{
						kmk_idkelompok = reader["kmk_idkelompok"].ToString(),
						kmk_namakelompok = reader["kmk_namakelompok"].ToString(),
						kmk_nim = reader["kmk_nim"].ToString(),
						kmk_idruangan = reader["kmk_idruangan"].ToString(),
						kmk_idpkkmb = reader["kmk_idpkkmb"].ToString(),
						kmk_status = reader["kmk_status"].ToString(),
					};
					kelList.Add(kel);
				}
				reader.Close();
				_connection.Close();
				return kelList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public KelompokModel getData(string kmk_idkelompok)
		{
			KelompokModel kel = new KelompokModel();
			try
			{
				string query = "SELECT * FROM pkm_mskelompok WHERE kmk_idkelompok = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", kmk_idkelompok);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					kel = new KelompokModel
					{
						kmk_idkelompok = reader["kmk_idkelompok"].ToString(),
						kmk_namakelompok = reader["kmk_namakelompok"].ToString(),
						kmk_nim = reader["kmk_nim"].ToString(),
						kmk_idruangan = reader["kmk_idruangan"].ToString(),
						kmk_idpkkmb = reader["kmk_idpkkmb"].ToString(),
						kmk_status = reader["kmk_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return kel;
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

		public KelompokModel getDataByNim(string kmk_nim)
		{
			KelompokModel kel = new KelompokModel();
			try
			{
				string query = "SELECT * FROM pkm_mskelompok WHERE kmk_nim = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", kmk_nim);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					kel = new KelompokModel
					{
						kmk_idkelompok = reader["kmk_idkelompok"].ToString(),
						kmk_namakelompok = reader["kmk_namakelompok"].ToString(),
						kmk_nim = reader["kmk_nim"].ToString(),
						kmk_idruangan = reader["kmk_idruangan"].ToString(),
						kmk_idpkkmb = reader["kmk_idpkkmb"].ToString(),
						kmk_status = reader["kmk_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return kel;
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

		public ResponseModel TambahKelompok([FromBody] KelompokModel kel)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_TambahKelompok", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@kmk_namakelompok", kel.kmk_namakelompok);
				command.Parameters.AddWithValue("@kmk_nim", kel.kmk_nim);
				command.Parameters.AddWithValue("@kmk_idruangan", kel.kmk_idruangan);
				command.Parameters.AddWithValue("@kmk_idpkkmb", kel.kmk_idpkkmb);
				command.Parameters.AddWithValue("@kmk_status", "Aktif");

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Kelompok berhasil dibuat";
				response.data = kel;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat membuat Kelompok = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel UpdateKelompok([FromBody] KelompokModel kel)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_UpdateKelompok", _connection);
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@kmk_idkelompok", kel.kmk_idkelompok);
				command.Parameters.AddWithValue("@kmk_namakelompok", kel.kmk_namakelompok);
				command.Parameters.AddWithValue("@kmk_nim", kel.kmk_nim);
				command.Parameters.AddWithValue("@kmk_idruangan", kel.kmk_idruangan);
				command.Parameters.AddWithValue("@kmk_idpkkmb", kel.kmk_idpkkmb);
				command.Parameters.AddWithValue("@kmk_status", kel.kmk_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Kelompok berhasil diubah";
				response.data = kel;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengubah Kelompok = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public List<MahasiswaBaruModel> getAnggotaKelompok(string idkelompok)
		{
			List<MahasiswaBaruModel> mhsList = new List<MahasiswaBaruModel>();
			try
			{
				string query = "SELECT * FROM GetAnggotaKelompok(@p_idkelompok)";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p_idkelompok", idkelompok);
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
								: DateTime.MinValue
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
	}
}
