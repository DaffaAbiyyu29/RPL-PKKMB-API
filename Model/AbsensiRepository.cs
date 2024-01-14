using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class AbsensiRepository
	{
		private readonly string _connectingString;
		private readonly SqlConnection _connection;
		ResponseModel response = new ResponseModel();
		private readonly IConfiguration _configuration;

		public AbsensiRepository(IConfiguration configuration)
		{
			_connectingString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectingString);
			_configuration = configuration;
		}

		public List<AbsensiModel> getAllData()
		{
			List<AbsensiModel> absenList = new List<AbsensiModel>();
			try
			{
				string query = "Select * from pkm_trabsensi";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					AbsensiModel absensi = new AbsensiModel
					{
						abs_idabsensi = reader["abs_idabsensi"].ToString(),
						abs_nim = reader["abs_nim"].ToString(),
						abs_nopendaftaran = reader["abs_nopendaftaran"].ToString(),
						abs_tglkehadiran = DateTime.Parse(reader["abs_tglkehadiran"].ToString()),
						abs_statuskehadiran = reader["abs_Statuskehadiran"].ToString(),
						abs_keterangan = reader["abs_keterangan"].ToString(),
						abs_idpkkmb = reader["abs_idpkkmb"].ToString(),
						abs_status = reader["abs_status"].ToString(),
					};
					absenList.Add(absensi);
				}
				reader.Close();
				_connection.Close();
				return absenList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public AbsensiModel getData([FromBody] string abs_idabsensi)
		{
			AbsensiModel absensi = new AbsensiModel();
			try
			{
				string query = "select * from pkm_trabsensi where abs_idabsensi = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", abs_idabsensi);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					absensi = new AbsensiModel
					{
						abs_idabsensi = reader["abs_idabsensi"].ToString(),
						abs_nim = reader["abs_nim"].ToString(),
						abs_nopendaftaran = reader["abs_nopendaftaran"].ToString(),
						abs_tglkehadiran = DateTime.Parse(reader["abs_tglkehadiran"].ToString()),
						abs_statuskehadiran = reader["abs_Statuskehadiran"].ToString(),
						abs_keterangan = reader["abs_keterangan"].ToString(),
                        abs_idpkkmb = reader["abs_idpkkmb"].ToString(),
						abs_status = reader["abs_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return absensi;
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

		public ResponseModel TambahAbsensi(List<AbsensiModel> absensi)
		{
			try
			{
				_connection.Open();
				foreach (var absensiItem in absensi)
				{
					//string query = "insert into pkm_trabsensi values (@p1,@p2,@p3,@p4,@p5,@p6, @p7)";
					SqlCommand command = new SqlCommand("sp_TambahAbsensi", _connection);
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@p_nim", absensiItem.abs_nim);
					command.Parameters.AddWithValue("@p_nopendaftaran", absensiItem.abs_nopendaftaran);
					command.Parameters.AddWithValue("@p_tglkehadiran", absensiItem.abs_tglkehadiran);
					command.Parameters.AddWithValue("@p_statuskehadiran", absensiItem.abs_statuskehadiran);
					command.Parameters.AddWithValue("@p_keterangan", absensiItem.abs_keterangan);
					command.Parameters.AddWithValue("@p_idpkkmb", absensiItem.abs_idpkkmb);
					command.Parameters.AddWithValue("@p_status", absensiItem.abs_status);

					command.ExecuteNonQuery();
				}
				//string message = (string)command.Parameters["@ErrorMessage"].Value;
				response.status = 200;
				response.messages = "Absensi Berhasil";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = ex.Message;
			}
			finally
			{
				_connection.Close();
			}

			return response;
		}

		public ResponseModel ubahAbsensi([FromBody] AbsensiModel absensi)
		{
			try
			{
				string query = "UPDATE pkm_trabsensi " +
							"SET abs_nim = @p2, " +
							"abs_nopendaftaran = @p3, " +
							"abs_tglkehadiran = @p4, " +
							"abs_statuskehadiran = @p5, " +
							"abs_keterangan = @p6, " +
							"abs_status = @p7 " +
							"WHERE abs_idabsensi= @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				/*command.CommandType = System.Data.CommandType.StoredProcedure;*/
				command.Parameters.AddWithValue("@p1", absensi.abs_idabsensi);
				command.Parameters.AddWithValue("@p2", absensi.abs_nim);
				command.Parameters.AddWithValue("@p3", absensi.abs_nopendaftaran);
				command.Parameters.AddWithValue("@p4", absensi.abs_tglkehadiran);
				command.Parameters.AddWithValue("@p5", absensi.abs_statuskehadiran);
				command.Parameters.AddWithValue("@p6", absensi.abs_keterangan);
				command.Parameters.AddWithValue("@p7", absensi.abs_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Absensi berhasil diubah";
				response.data = absensi;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat Absensi = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel hapusAbsensi([FromBody] string abs_idabsensi)
		{
			try
			{
				string query = "delete from pkm_trabsensi where abs_idabsensi= @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", abs_idabsensi);
				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "Absensi berhasil dihapus";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat menghapus Absensi = " + ex.Message;
			}

			return response;
		}

		public List<AbsensiModel> getAbsensiByTanggal(string abs_tglkehadiran)
		{
			List<AbsensiModel> absenList = new List<AbsensiModel>();
			try
			{
				string query = "SELECT * FROM [dbo].[GetAbsensiByTanggal](@p_tanggal)";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p_tanggal", abs_tglkehadiran);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					AbsensiModel absensi = new AbsensiModel
					{
						abs_idabsensi = reader["abs_idabsensi"].ToString(),
						abs_nim = reader["abs_nim"].ToString(),
						abs_nopendaftaran = reader["abs_nopendaftaran"].ToString(),
						abs_tglkehadiran = DateTime.Parse(reader["abs_tglkehadiran"].ToString()),
						abs_statuskehadiran = reader["abs_Statuskehadiran"].ToString(),
						abs_keterangan = reader["abs_keterangan"].ToString(),
						abs_status = reader["abs_status"].ToString(),
					};
					absenList.Add(absensi);
				}
				reader.Close();
				_connection.Close();
				return absenList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}
	}
}
