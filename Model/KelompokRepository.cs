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

		public List<KelompokModel> TampilKelompok()
		{
			List<KelompokModel> kelList = new List<KelompokModel>();
			try
			{
				string query = "SELECT * FROM pkm_mskelompok";
				SqlCommand command = new SqlCommand(query, _connection);
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

				command.Parameters.AddWithValue("@p1", kel.kmk_idkelompok);
				command.Parameters.AddWithValue("@p2", kel.kmk_namakelompok);
				command.Parameters.AddWithValue("@p3", kel.kmk_nim);
				command.Parameters.AddWithValue("@p4", kel.kmk_idruangan);

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
	}
}
