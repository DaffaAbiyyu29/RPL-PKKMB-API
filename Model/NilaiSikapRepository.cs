using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class NilaiSikapRepository
	{
		private readonly string _connectionString;

		private readonly SqlConnection _connection;
		ResponseModel responseModel = new ResponseModel();

		public NilaiSikapRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectionString);
		}
		public List<NilaiSikapModel> getAllData()
		{
			List<NilaiSikapModel> nilaisikapList = new List<NilaiSikapModel>();
			try
			{
				string query = "select * from pkm_trnilaisikap";
				SqlCommand command = new SqlCommand(query, _connection);

				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					NilaiSikapModel nilaiSikap = new NilaiSikapModel
					{
						nls_idnilaisikap = reader["nls_idnilaisikap"].ToString(),
						nls_nopendaftaran = reader["nls_nopendaftaran"].ToString(),
						nls_nim = reader["nls_nim"].ToString(),
						nls_sikap = reader["nls_sikap"].ToString(),
						nls_tanggal = DateTime.Parse(reader["nls_tanggal"].ToString()),
						nls_jamplus = Convert.ToInt32(reader["nls_jamplus"].ToString()),
						nls_jamminus = Convert.ToInt32(reader["nls_jamminus"].ToString()),
						nls_deskripsi = reader["nls_deskripsi"].ToString(),
						nls_status = reader["nls_status"].ToString(),
					};
					nilaisikapList.Add(nilaiSikap);
				}
				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return nilaisikapList;
		}

		public NilaiSikapModel getData(string nls_idnilaisikap)
		{
			NilaiSikapModel nilaiSikapModel = new NilaiSikapModel();
			try
			{
				string query = "select * from pkm_trnilaisikap where nls_idnilaisikap= @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", nls_idnilaisikap);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				reader.Read();
				nilaiSikapModel.nls_idnilaisikap = reader["nls_idnilaisikap"].ToString();
				nilaiSikapModel.nls_nopendaftaran = reader["nls_nopendaftaran"].ToString();
				nilaiSikapModel.nls_nim = reader["nls_nim"].ToString();
				nilaiSikapModel.nls_sikap = reader["nls_sikap"].ToString();
				nilaiSikapModel.nls_jamplus = Convert.ToInt32(reader["nls_jamplus"].ToString());
				nilaiSikapModel.nls_jamminus = Convert.ToInt32(reader["nls_jamminus"].ToString());
				nilaiSikapModel.nls_tanggal = DateTime.Parse(reader["nls_tanggal"].ToString());
				nilaiSikapModel.nls_deskripsi = reader["nls_deskripsi"].ToString();
				nilaiSikapModel.nls_status = reader["nls_status"].ToString();
				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return nilaiSikapModel;
		}


		public ResponseModel insertNilaiSikap([FromBody] NilaiSikapModel nilaiSikapModel)
		{
			try
			{
				//string query = "insert into pkm_trnilaisikap values(@p1,@p2, @p3, @p4, @p5, @p6, @p7, @p8)";
				SqlCommand command = new SqlCommand("sp_TambahNilaisikap", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nls_nopendaftaran", nilaiSikapModel.nls_nopendaftaran);
				command.Parameters.AddWithValue("@p_nls_nim", nilaiSikapModel.nls_nim);
				command.Parameters.AddWithValue("@p_nls_sikap", nilaiSikapModel.nls_sikap);
				command.Parameters.AddWithValue("@p_nls_tanggal", nilaiSikapModel.nls_tanggal);
				command.Parameters.AddWithValue("@p_nls_jamplus", nilaiSikapModel.nls_jamplus);
				command.Parameters.AddWithValue("@p_nls_jamminus", nilaiSikapModel.nls_jamminus);
				command.Parameters.AddWithValue("@p_nls_deskripsi", nilaiSikapModel.nls_deskripsi);
				command.Parameters.AddWithValue("@p_nls_status", nilaiSikapModel.nls_status);
				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				responseModel.status = 500;
				responseModel.messages = "Failed, " + ex.Message;
				return responseModel;
			}
			responseModel.status = 200;
			responseModel.messages = "Success";
			responseModel.data = null;

			return responseModel;
		}
		public ResponseModel updateNilaiSikap(NilaiSikapModel nilaiSikapModel)
		{
			try
			{
				string query = "UPDATE pkm_trnilaisikap " +
							"SET nls_nopendaftaran = @p2, " +
							"nls_nim = @p3, " +
							"nls_sikap = @p4, " +
							"nls_jamplus = @p5, " +
							"nls_jamminus = @p6, " +
							"nls_tanggal = @p7, " +
							"nls_deskripsi = @p8 " +
							"nls_status = @p9 " +
							"WHERE nls_idnilaisikap = @p1";
				using SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", nilaiSikapModel.nls_idnilaisikap);
				command.Parameters.AddWithValue("@p2", nilaiSikapModel.nls_nopendaftaran);
				command.Parameters.AddWithValue("@p3", nilaiSikapModel.nls_nim);
				command.Parameters.AddWithValue("@p4", nilaiSikapModel.nls_sikap);
				command.Parameters.AddWithValue("@p5", nilaiSikapModel.nls_jamplus);
				command.Parameters.AddWithValue("@p6", nilaiSikapModel.nls_jamminus);
				command.Parameters.AddWithValue("@p7", nilaiSikapModel.nls_tanggal);
				command.Parameters.AddWithValue("@p8", nilaiSikapModel.nls_deskripsi);
				command.Parameters.AddWithValue("@p9", nilaiSikapModel.nls_status);
				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				responseModel.status = 500;
				responseModel.messages = "Failed, " + ex.Message;
				return responseModel;
			}
			responseModel.status = 200;
			responseModel.messages = "Success";
			responseModel.data = null;

			return responseModel;
		}
	}
}