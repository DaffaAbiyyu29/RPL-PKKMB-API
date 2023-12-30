using System.Data;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class JadwalRepository
	{
		private readonly string _connectionString;

		private readonly SqlConnection _connection;
		ResponseModel response = new ResponseModel();

		public JadwalRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectionString);
		}
		public List<JadwalModel> getAllData()
		{
			List<JadwalModel> jadwalList = new List<JadwalModel>();
			try
			{
				string query = "select * from pkm_msjadwal";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					JadwalModel jadwal = new JadwalModel
					{
						jdl_idjadwal = reader["jdl_idjadwal"].ToString(),
						jdl_nim = reader["jdl_nim"].ToString(),
						jdl_tglpelaksanaan = DateTime.Parse(reader["jdl_tglpelaksanaan"].ToString()),
						jdl_waktupelaksanaan = reader["jdl_waktupelaksanaan"].ToString(),
						jdl_agenda = reader["jdl_agenda"].ToString(),
						jdl_tempat = reader["jdl_tempat"].ToString(),
						jdl_status = reader["jdl_status"].ToString(),
					};
					jadwalList.Add(jadwal);
				}
				reader.Close();
				_connection.Close();
				return jadwalList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public JadwalModel getData(string jdl_idjadwal)
		{
			JadwalModel jdl = new JadwalModel();
			try
			{
				string query = "select * from pkm_msjadwal where jdl_idjadwal = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", jdl_idjadwal);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					jdl = new JadwalModel
					{
						jdl_idjadwal = reader["jdl_idjadwal"].ToString(),
						jdl_nim = reader["jdl_nim"].ToString(),
						jdl_tglpelaksanaan = DateTime.Parse(reader["jdl_tglpelaksanaan"].ToString()),
						jdl_waktupelaksanaan = reader["jdl_waktupelaksanaan"].ToString(),
						jdl_agenda = reader["jdl_agenda"].ToString(),
						jdl_tempat = reader["jdl_tempat"].ToString(),
						jdl_status = reader["jdl_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return jdl;
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

		public ResponseModel insertJadwal(JadwalModel jadwalModel)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_TambahJadwal", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nim", jadwalModel.jdl_nim);
				command.Parameters.AddWithValue("@p_tglpelaksanaan", jadwalModel.jdl_tglpelaksanaan);
				command.Parameters.AddWithValue("@p_waktupelaksanaan", jadwalModel.jdl_waktupelaksanaan);
				command.Parameters.AddWithValue("@p_agenda", jadwalModel.jdl_agenda);
				command.Parameters.AddWithValue("@p_tempat", jadwalModel.jdl_tempat);
				command.Parameters.AddWithValue("@p_status", jadwalModel.jdl_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Jadwal berhasil dibuat";
				response.data = jadwalModel;
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

		public ResponseModel UpdateJadwal(JadwalModel jdl)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_UpdateJadwal", _connection);
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@p_idjadwal", jdl.jdl_idjadwal);
				command.Parameters.AddWithValue("@p_nim", jdl.jdl_nim);
				command.Parameters.AddWithValue("@p_tglpelaksanaan", jdl.jdl_tglpelaksanaan);
				command.Parameters.AddWithValue("@p_waktupelaksanaan", jdl.jdl_waktupelaksanaan);
				command.Parameters.AddWithValue("@p_agenda", jdl.jdl_agenda);
				command.Parameters.AddWithValue("@p_tempat", jdl.jdl_tempat);
				command.Parameters.AddWithValue("@p_status", jdl.jdl_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Success";
				response.data = jdl;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Failed, " + ex.Message;
				return response;
			}

			return response;
		}
	}
}
