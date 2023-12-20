using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
	public class PicPkkmbRepository
	{
		private readonly string _connectingString;
		private readonly SqlConnection _connection;
		ResponseModel response = new ResponseModel();

		public PicPkkmbRepository(IConfiguration configuration)
		{
			_connectingString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectingString);
		}

		public List<PicPkkmbModel> getAllData()
		{
			List<PicPkkmbModel> picList = new List<PicPkkmbModel>();
			try
			{
				string query = "select * from pkm_mspicpkkmb";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					PicPkkmbModel pic = new PicPkkmbModel
					{
						pic_nokaryawan = reader["pic_nokaryawan"].ToString(),
						pic_nama = reader["pic_nama"].ToString(),
						pic_password = reader["pic_password"].ToString(),
						pic_status = reader["pic_status"].ToString(),
					};
					picList.Add(pic);
				}
				reader.Close();
				_connection.Close();
				return picList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
				return null;
			}
		}

		public PicPkkmbModel getData(string pic_nokaryawan)
		{
			PicPkkmbModel pic = new PicPkkmbModel();
			try
			{
				string query = "SELECT * FROM pkm_mspicpkkmb WHERE pic_nokaryawan = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", pic_nokaryawan);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					pic = new PicPkkmbModel
					{
						pic_nokaryawan = reader["pic_nokaryawan"].ToString(),
						pic_nama = reader["pic_nama"].ToString(),
						pic_password = reader["pic_password"].ToString(),
						pic_status = reader["pic_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return pic;
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

		public PicPkkmbModel login(string pic_nokaryawan, string password)
		{
			PicPkkmbModel pic = new PicPkkmbModel();
			try
			{
				string query = "SELECT * FROM pkm_mspicpkkmb WHERE pic_nokaryawan = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", pic_nokaryawan);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					// User found, verify password
					string hashedPasswordFromDb = reader["pic_password"].ToString();

					if (!string.IsNullOrEmpty(hashedPasswordFromDb))
					{
						bool verify = BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb);

						if (verify)
						{
							// Password is correct, populate PicPkkmbModel
							pic = new PicPkkmbModel
							{
								pic_nokaryawan = reader["pic_nokaryawan"].ToString(),
								pic_nama = reader["pic_nama"].ToString(),
								pic_password = reader["pic_password"].ToString(),
								pic_status = reader["pic_status"].ToString(),
							};
						}
					}

					reader.Close();
					_connection.Close();
					return pic;
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

		public ResponseModel daftarPIC([FromBody] PicPkkmbModel pic)
		{
			try
			{
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(pic.pic_password, 12);

				string query = "INSERT INTO pkm_mspicpkkmb (pic_nokaryawan, pic_nama, pic_password, pic_status) VALUES (@p1, @p2, @p3, @p4)";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", pic.pic_nokaryawan);
				command.Parameters.AddWithValue("@p2", pic.pic_nama);
				command.Parameters.AddWithValue("@p3", hashedPassword);
				command.Parameters.AddWithValue("@p4", "Aktif");
				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "PIC berhasil didaftarkan";
				response.data = pic;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mendaftarkan PIC = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel updatePIC([FromBody] PicPkkmbModel pic)
		{
			try
			{
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(pic.pic_password, 12);

				string query = "UPDATE pkm_mspicpkkmb " +
							"SET pic_nama = @p2, " +
							"pic_password = @p3 " +
							"WHERE pic_nokaryawan= @p1";
				using SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", pic.pic_nokaryawan);
				command.Parameters.AddWithValue("@p2", pic.pic_nama);
				command.Parameters.AddWithValue("@p3", hashedPassword);


				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "PIC berhasil diubah";
				response.data = pic;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat mengubah PIC = " + ex.Message;
				response.data = null;
			}

			return response;
		}

		public ResponseModel deletePIC([FromBody] string pic_nokaryawan)
		{
			try
			{
				string query = "delete from pkm_mspicpkkmb where pic_nokaryawan= @p1";
				using SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", pic_nokaryawan);
				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				response.status = 200;
				response.messages = "PIC berhasil dihapus";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				response.status = 500;
				response.messages = "Terjadi kesalahan saat menghapus PIC = " + ex.Message;
			}

			return response;
		}
	}
}
