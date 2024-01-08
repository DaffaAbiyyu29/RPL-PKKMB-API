using System.Data;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class InformasiRepository
	{
		private readonly string _connectionString;

		private readonly SqlConnection _connection;
		ResponseModel responseModel = new ResponseModel();

		public InformasiRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectionString);
		}
		public List<InformasiModel> getAllData()
		{
			List<InformasiModel> jadwalList = new List<InformasiModel>();
			try
			{
				string query = "select * from pkm_msinformasi";
				SqlCommand command = new SqlCommand(query, _connection);

				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					InformasiModel informasi = new InformasiModel
					{
						inf_idinformasi = reader["inf_idinformasi"].ToString(),
						inf_jenisinformasi = reader["inf_jenisinformasi"].ToString(),
						inf_namainformasi = reader["inf_namainformasi"].ToString(),
						inf_tglpublikasi = DateTime.Parse(reader["inf_tglpublikasi"].ToString()),
						inf_deskripsi = reader["inf_deskripsi"].ToString(),
						inf_status = reader["inf_status"].ToString()

					};
					jadwalList.Add(informasi);
				}
				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return jadwalList;
		}

		public InformasiModel GetData(string inf_idinformasi)
		{
			InformasiModel informasiModel = new InformasiModel();

			try
			{
				string storedProcedureName = "sp_GetInformasi";
				SqlCommand command = new SqlCommand(storedProcedureName, _connection);
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@inf_idinformasi", inf_idinformasi);
				_connection.Open();

				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					informasiModel.inf_idinformasi = reader["inf_idinformasi"].ToString();
					informasiModel.inf_jenisinformasi = reader["inf_jenisinformasi"].ToString();
					informasiModel.inf_namainformasi = reader["inf_namainformasi"].ToString();
					informasiModel.inf_tglpublikasi = DateTime.Parse(reader["inf_tglpublikasi"].ToString());
					informasiModel.inf_deskripsi = reader["inf_deskripsi"].ToString();
					informasiModel.inf_status = reader["inf_status"].ToString();
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

			return informasiModel;
		}


		public ResponseModel InsertInformasi(InformasiModel informasiModel)
		{
			ResponseModel responseModel = new ResponseModel();

			try
			{
				string storedProcedureName = "sp_InsertInformasi";
				SqlCommand command = new SqlCommand(storedProcedureName, _connection);
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@inf_jenisinformasi", informasiModel.inf_jenisinformasi);
				command.Parameters.AddWithValue("@inf_namainformasi", informasiModel.inf_namainformasi);
				command.Parameters.AddWithValue("@inf_tglpublikasi", informasiModel.inf_tglpublikasi);
				command.Parameters.AddWithValue("@inf_deskripsi", informasiModel.inf_deskripsi);
				command.Parameters.AddWithValue("@inf_status", informasiModel.inf_status);

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

		public ResponseModel UpdateInformasi(InformasiModel informasiModel)
		{
			ResponseModel responseModel = new ResponseModel();

			try
			{
				string storedProcedureName = "sp_UpdateInformasi";
				using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@inf_idinformasi", informasiModel.inf_idinformasi);
				command.Parameters.AddWithValue("@inf_jenisinformasi", informasiModel.inf_jenisinformasi);
				command.Parameters.AddWithValue("@inf_namainformasi", informasiModel.inf_namainformasi);
				command.Parameters.AddWithValue("@inf_tglpublikasi", informasiModel.inf_tglpublikasi);
				command.Parameters.AddWithValue("@inf_deskripsi", informasiModel.inf_deskripsi);
				command.Parameters.AddWithValue("@inf_status", informasiModel.inf_status);

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