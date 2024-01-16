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
						nls_idpkkmb = reader["nls_idpkkmb"].ToString(),
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
				nilaiSikapModel.nls_tanggal = DateTime.Parse(reader["nls_tanggal"].ToString());
				nilaiSikapModel.nls_idpkkmb = reader["nls_idpkkmb"].ToString();
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
				command.Parameters.AddWithValue("@p_nls_idpkkmb", nilaiSikapModel.nls_idpkkmb);
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
				SqlCommand command = new SqlCommand("sp_UpdateNilaiSikap", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@nls_idnilaisikap", nilaiSikapModel.nls_idnilaisikap);
				command.Parameters.AddWithValue("@nls_nopendaftaran", nilaiSikapModel.nls_nopendaftaran);
				command.Parameters.AddWithValue("@nls_nim", nilaiSikapModel.nls_nim);
				command.Parameters.AddWithValue("@nls_sikap", nilaiSikapModel.nls_sikap);
				command.Parameters.AddWithValue("@nls_tanggal", nilaiSikapModel.nls_tanggal);
				command.Parameters.AddWithValue("@p_nls_idpkkmb", nilaiSikapModel.nls_idpkkmb);
				command.Parameters.AddWithValue("@nls_status", nilaiSikapModel.nls_status);
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

		public ResponseModel insertJam([FromBody] DetailJamModel jam)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_TambahJamPlusMinus", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nopendaftaran", jam.dtj_nopendaftaran);
				command.Parameters.AddWithValue("@p_nim", jam.dtj_nim);
				command.Parameters.AddWithValue("@p_jenisjam", jam.dtj_jenisjam);
				command.Parameters.AddWithValue("@p_jumlah", jam.dtj_jumlah);
				command.Parameters.AddWithValue("@p_deskripsi", jam.dtj_deskripsi);
				command.Parameters.AddWithValue("@p_tanggal", jam.dtj_tanggal);
				command.Parameters.AddWithValue("@p_idpkkmb", jam.dtj_idpkkmb);
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

		public ResponseModel ubahJam([FromBody] DetailJamModel jam)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_UpdateJamPlusMinus", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_idjam", jam.dtj_idjam);
				command.Parameters.AddWithValue("@p_nopendaftaran", jam.dtj_nopendaftaran);
				command.Parameters.AddWithValue("@p_nim", jam.dtj_nim);
				command.Parameters.AddWithValue("@p_jenisjam", jam.dtj_jenisjam);
				command.Parameters.AddWithValue("@p_jumlah", jam.dtj_jumlah);
				command.Parameters.AddWithValue("@p_deskripsi", jam.dtj_deskripsi);
				command.Parameters.AddWithValue("@p_tanggal", jam.dtj_tanggal);
				command.Parameters.AddWithValue("@p_idpkkmb", jam.dtj_idpkkmb);
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

		public List<DetailJamModel> getDetailJamMahasiswa(string dtj_nopendaftaran)
		{
			List<DetailJamModel> dtjList = new List<DetailJamModel>();
			try
			{
				string query = "SELECT * FROM pkm_trdetailjamplusminus WHERE dtj_nopendaftaran = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", dtj_nopendaftaran);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					DetailJamModel dtj = new DetailJamModel
					{
						dtj_idjam = reader["dtj_idjam"].ToString(),
						dtj_nopendaftaran = reader["dtj_nopendaftaran"].ToString(),
						dtj_nim = reader["dtj_nim"].ToString(),
						dtj_jenisjam = reader["dtj_jenisjam"].ToString(),
						dtj_jumlah = int.Parse(reader["dtj_jumlah"].ToString()),
						dtj_tanggal = DateTime.Parse(reader["dtj_tanggal"].ToString()),
						dtj_deskripsi = reader["dtj_deskripsi"].ToString(),
						dtj_idpkkmb = reader["dtj_idpkkmb"].ToString(),
						dtj_status = reader["dts_status"].ToString(),
					};
					dtjList.Add(dtj);
				}
				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return dtjList;
		}

		public DetailJamModel getDataDetailJamMahasiswa(string dtj_idjam, string dtj_nopendaftaran)
		{
			DetailJamModel dtj = new DetailJamModel();
			try
			{
				string query = "SELECT * FROM pkm_trdetailjamplusminus WHERE dtj_idjam = @p1 AND dtj_nopendaftaran = @p2";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", dtj_idjam);
				command.Parameters.AddWithValue("@p2", dtj_nopendaftaran);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				reader.Read();

				dtj.dtj_idjam = reader["dtj_idjam"].ToString();
				dtj.dtj_nopendaftaran = reader["dtj_nopendaftaran"].ToString();
				dtj.dtj_nim = reader["dtj_nim"].ToString();
				dtj.dtj_jenisjam = reader["dtj_jenisjam"].ToString();
				dtj.dtj_jumlah = int.Parse(reader["dtj_jumlah"].ToString());
				dtj.dtj_tanggal = DateTime.Parse(reader["dtj_tanggal"].ToString());
				dtj.dtj_deskripsi = reader["dtj_deskripsi"].ToString();
				dtj.dtj_idpkkmb = reader["dtj_idpkkmb"].ToString();
				dtj.dtj_status = reader["dts_status"].ToString();

				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return dtj;
		}
	}
}