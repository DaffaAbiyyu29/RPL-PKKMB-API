using System.Data;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class RuanganRepository
	{
		private readonly string _connectionString;

		private readonly SqlConnection _connection;
		ResponseModel responseModel = new ResponseModel();

		public RuanganRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectionString);
		}
		public List<RuanganModel> getAllData()
		{
			List<RuanganModel> ruanganList = new List<RuanganModel>();
			try
			{
				string query = "select * from pkm_msruangan";
				SqlCommand command = new SqlCommand(query, _connection);

				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					RuanganModel ruangan = new RuanganModel
					{
						rng_idruangan = reader["rng_idruangan"].ToString(),
						rng_namaruangan = reader["rng_namaruangan"].ToString(),
						rng_idpkkmb = reader["rng_idpkkmb"].ToString(),
						rng_status = reader["rng_status"].ToString(),
					};
					ruanganList.Add(ruangan);
				}
				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ruanganList;
		}

		public RuanganModel getData(string rng_idruangan)
		{
			RuanganModel ruanganModel = new RuanganModel();
			try
			{
				string query = "select * from pkm_msruangan where rng_idruangan= @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", rng_idruangan);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				reader.Read();
				ruanganModel.rng_idruangan = reader["rng_idruangan"].ToString();
				ruanganModel.rng_namaruangan = reader["rng_namaruangan"].ToString();
				ruanganModel.rng_idpkkmb = reader["rng_idpkkmb"].ToString();
				ruanganModel.rng_status = reader["rng_status"].ToString();
				reader.Close();
				_connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ruanganModel;
		}

		public ResponseModel insertRuangan(RuanganModel ruanganModel)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_InsertRuangan", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@rng_namaruangan", ruanganModel.rng_namaruangan);
				command.Parameters.AddWithValue("@rng_idpkkmb", ruanganModel.rng_idpkkmb);
				command.Parameters.AddWithValue("@rng_status", ruanganModel.rng_status);
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

		public ResponseModel updateRuangan(RuanganModel ruanganModel)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_UpdateRuangan", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@rng_idruangan", ruanganModel.rng_idruangan);
				command.Parameters.AddWithValue("@rng_namaruangan", ruanganModel.rng_namaruangan);
				command.Parameters.AddWithValue("@rng_idpkkmb", ruanganModel.rng_idpkkmb);
				command.Parameters.AddWithValue("@rng_status", ruanganModel.rng_status);
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
