using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PKKMB_API.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace PKKMB_API.Repository
{
	public class TugasRepository
	{
		private readonly string _connectionString;
		private readonly SqlConnection _connection;
		private readonly ResponseModel _response = new ResponseModel();

		public TugasRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectionString);
		}

		public List<TugasModel> GetAllData()
		{
			List<TugasModel> tugasList = new List<TugasModel>();
			try
			{
				string query = "SELECT * FROM pkm_trtugas";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					TugasModel tugas = new TugasModel
					{
						tgs_idtugas = reader["tgs_idtugas"].ToString(),
						tgs_nim = reader["tgs_nim"].ToString(),
						tgs_namatugas = reader["tgs_namatugas"].ToString(),
						tgs_tglpemberiantugas = DateTime.Parse(reader["tgs_tglpemberiantugas"].ToString()),
						tgs_filetugas = reader["tgs_filetugas"].ToString(),
						tgs_deadline = DateTime.Parse(reader["tgs_deadline"].ToString()),
						tgs_deskripsi = reader["tgs_deskripsi"].ToString(),
						tgs_status = reader["tgs_status"].ToString(),
					};
					tugasList.Add(tugas);
				}
				reader.Close();
				_connection.Close();
				return tugasList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public string NextId()
		{
			try
			{
				string query = "SELECT dbo.GenerateTgsIdTugas() AS ID";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				string nextId = null;

				if (reader.Read())
				{
					nextId = reader["ID"].ToString();
				}

				reader.Close();
				_connection.Close();
				return nextId;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null; // atau throw exception sesuai kebutuhan
			}
		}

		public string GetOldFile(string tgs_idtugas)
		{
			try
			{
				string query = "SELECT tgs_filetugas FROM pkm_trtugas WHERE tgs_idtugas = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", tgs_idtugas);
				_connection.Open();

				SqlDataReader reader = command.ExecuteReader();
				string oldFile = null;

				if (reader.Read())
				{
					oldFile = reader["tgs_filetugas"].ToString();
				}

				reader.Close();
				_connection.Close();
				return oldFile;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null; // atau throw exception sesuai kebutuhan
			}
		}

		public string GetOldFileDetail(string dts_idtugas, string dts_nopendaftaran)
		{
			try
			{
				string query = "SELECT dts_filetugas FROM pkm_trdetailtugas WHERE dts_iddetail = @p1 AND dts_nopendaftaran = @p2";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", dts_idtugas);
				command.Parameters.AddWithValue("@p2", dts_nopendaftaran);
				_connection.Open();

				SqlDataReader reader = command.ExecuteReader();
				string oldFile = null;

				if (reader.Read())
				{
					oldFile = reader["dts_filetugas"].ToString();
				}

				reader.Close();
				_connection.Close();
				return oldFile;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public TugasModel GetData(string tgs_idtugas)
		{
			TugasModel tugas = new TugasModel();
			try
			{
				string query = "SELECT * FROM pkm_trtugas WHERE tgs_idtugas = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", tgs_idtugas);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					tugas = new TugasModel
					{
						tgs_idtugas = reader["tgs_idtugas"].ToString(),
						tgs_nim = reader["tgs_nim"].ToString(),
						tgs_namatugas = reader["tgs_namatugas"].ToString(),
						tgs_tglpemberiantugas = DateTime.Parse(reader["tgs_tglpemberiantugas"].ToString()),
						tgs_filetugas = reader["tgs_filetugas"].ToString(),
						tgs_deadline = DateTime.Parse(reader["tgs_deadline"].ToString()),
						tgs_deskripsi = reader["tgs_deskripsi"].ToString(),
						tgs_status = reader["tgs_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return tugas;
				}
				else
				{
					// Tugas not found
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

		public ResponseModel InsertTugas(TugasModel tugasModel)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_TambahTugas", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nim", tugasModel.tgs_nim);
				command.Parameters.AddWithValue("@p_namatugas", tugasModel.tgs_namatugas);
				command.Parameters.AddWithValue("@p_tglpemberiantugas", tugasModel.tgs_tglpemberiantugas);
				command.Parameters.AddWithValue("@p_filetugas", tugasModel.tgs_filetugas);
				command.Parameters.AddWithValue("@p_deadline", tugasModel.tgs_deadline);
				command.Parameters.AddWithValue("@p_deskripsi", tugasModel.tgs_deskripsi);
				command.Parameters.AddWithValue("@p_status", tugasModel.tgs_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil ditambahkan";
				_response.data = tugasModel;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Terjadi kesalahan saat menambahkan Tugas = " + ex.Message;
				_response.data = null;
			}

			return _response;
		}

		public ResponseModel UpdateTugas(TugasModel tugas)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_UpdateTugasWithoutFile", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_idtugas", tugas.tgs_idtugas);
				command.Parameters.AddWithValue("@p_nim", tugas.tgs_nim);
				command.Parameters.AddWithValue("@p_namatugas", tugas.tgs_namatugas);
				command.Parameters.AddWithValue("@p_tglpemberiantugas", tugas.tgs_tglpemberiantugas);
				command.Parameters.AddWithValue("@p_deadline", tugas.tgs_deadline);
				command.Parameters.AddWithValue("@p_deskripsi", tugas.tgs_deskripsi);
				command.Parameters.AddWithValue("@p_status", tugas.tgs_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil diubah";
				_response.data = tugas;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Gagal mengupdate Tugas, " + ex.Message;
				return _response;
			}

			return _response;
		}

		public ResponseModel UploadFile(string tgs_nim, string tgs_namatugas, DateTime tgs_tglpemberiantugas, IFormFile file, DateTime tgs_deadline, string tgs_deskripsi)
		{
			try
			{
				// Mendapatkan direktori tempat menyimpan file
				string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "File_Tugas\\TugasMahasiswa");

				// Mengecek apakah direktori sudah ada, jika belum, maka membuatnya
				if (!Directory.Exists(uploadDir))
				{
					Directory.CreateDirectory(uploadDir);
				}

				// Membuat nama file yang unik untuk menghindari konflik
				string uniqueFileName = file.FileName;

				// Membuat path file tujuan
				string filePath = Path.Combine(uploadDir, uniqueFileName);

				// Menyimpan file ke server
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				SqlCommand command = new SqlCommand("sp_TambahTugas", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nim", tgs_nim);
				command.Parameters.AddWithValue("@p_namatugas", tgs_namatugas);
				command.Parameters.AddWithValue("@p_tglpemberiantugas", tgs_tglpemberiantugas);
				command.Parameters.AddWithValue("@p_filetugas", uniqueFileName);
				command.Parameters.AddWithValue("@p_deadline", tgs_deadline);
				command.Parameters.AddWithValue("@p_deskripsi", tgs_deskripsi);
				command.Parameters.AddWithValue("@p_status", "Aktif");

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil diunggah";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Terjadi kesalahan saat mengunggah file: " + ex.Message;
				_response.data = null;
			}

			return _response;
		}

		public ResponseModel UbahFile(string tgs_idtugas, string tgs_nim, string tgs_namatugas, DateTime tgs_tglpemberiantugas, IFormFile file, DateTime tgs_deadline, string tgs_deskripsi, string tgs_status)
		{
			try
			{
				// Mendapatkan direktori tempat menyimpan file
				string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "File_Tugas\\TugasMahasiswa");
				string oldFile = GetOldFile(tgs_idtugas);
				string oldFilePath = Path.Combine(uploadDir, oldFile);

				// Mengecek apakah direktori sudah ada, jika belum, maka membuatnya
				if (!Directory.Exists(uploadDir))
				{
					Directory.CreateDirectory(uploadDir);
				}

				// Membuat nama file yang unik untuk menghindari konflik
				string uniqueFileName;

				if (file != null)
				{
					// Jika file tidak null, gunakan nama file baru
					uniqueFileName = file.FileName;

					// Membuat path file tujuan
					string filePath = Path.Combine(uploadDir, uniqueFileName);

					// Menyimpan file ke server
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					// Hapus file lama jika ada
					if (System.IO.File.Exists(oldFilePath))
					{
						System.IO.File.Delete(oldFilePath);
					}
				}
				else
				{
					// Jika file null, gunakan nama file lama
					uniqueFileName = oldFile;
				}

				SqlCommand command = new SqlCommand("sp_UpdateTugas", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_idtugas", tgs_idtugas);
				command.Parameters.AddWithValue("@p_nim", tgs_nim);
				command.Parameters.AddWithValue("@p_namatugas", tgs_namatugas);
				command.Parameters.AddWithValue("@p_tglpemberiantugas", tgs_tglpemberiantugas);

				// Pass the correct file name to the stored procedure
				command.Parameters.AddWithValue("@p_filetugas", uniqueFileName);

				command.Parameters.AddWithValue("@p_deadline", tgs_deadline);
				command.Parameters.AddWithValue("@p_deskripsi", tgs_deskripsi);
				command.Parameters.AddWithValue("@p_status", tgs_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil diubah";
				_response.data = oldFile;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Terjadi kesalahan saat mengubah tugas: " + ex.Message;
				_response.data = null;
			}

			return _response;
		}

		public List<DetailTugasModel> GetAllDataDetail()
		{
			List<DetailTugasModel> detailList = new List<DetailTugasModel>();
			try
			{
				string query = "SELECT * FROM pkm_trdetailtugas";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					DetailTugasModel detail = new DetailTugasModel
					{
						dts_iddetail = reader["dts_iddetail"].ToString(),
						dts_nopendaftaran = reader["dts_nopendaftaran"].ToString(),
						dts_filetugas = reader["dts_filetugas"].ToString(),
						dts_waktupengumpulan = DateTime.Parse(reader["dts_waktupengumpulan"].ToString()),
						dts_nilaitugas = Double.Parse(reader["dts_nilaitugas"].ToString()),
					};
					detailList.Add(detail);
				}
				reader.Close();
				_connection.Close();
				return detailList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public DetailTugasModel GetDataDetail(string dts_iddetail, string dts_nopendaftaran)
		{
			DetailTugasModel detail = new DetailTugasModel();
			try
			{
				string query = "SELECT * FROM pkm_trdetailtugas WHERE dts_iddetail = @p1 AND dts_nopendaftaran = @p2";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", dts_iddetail);
				command.Parameters.AddWithValue("@p2", dts_nopendaftaran);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					detail = new DetailTugasModel
					{
						dts_iddetail = reader["dts_iddetail"].ToString(),
						dts_nopendaftaran = reader["dts_nopendaftaran"].ToString(),
						dts_filetugas = reader["dts_filetugas"].ToString(),
						dts_waktupengumpulan = DateTime.Parse(reader["dts_waktupengumpulan"].ToString()),
						dts_nilaitugas = Double.Parse(reader["dts_nilaitugas"].ToString()),
					};

					reader.Close();
					_connection.Close();
					return detail;
				}
				else
				{
					// Tugas not found
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

		public ResponseModel UploadTugasMahasiswa(string dts_iddetail, string dts_nopendaftaran, IFormFile file, DateTime dts_waktupengumpulan, double dts_nilaitugas)
		{
			try
			{
				// Mendapatkan direktori tempat menyimpan file
				string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "File_Tugas\\PengumpulanTugas");

				// Mengecek apakah direktori sudah ada, jika belum, maka membuatnya
				if (!Directory.Exists(uploadDir))
				{
					Directory.CreateDirectory(uploadDir);
				}

				// Membuat nama file yang unik untuk menghindari konflik
				string uniqueFileName = file.FileName;

				// Membuat path file tujuan
				string filePath = Path.Combine(uploadDir, uniqueFileName);

				// Menyimpan file ke server
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				SqlCommand command = new SqlCommand("sp_TambahTugasDetail", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_idtugas", dts_iddetail);
				command.Parameters.AddWithValue("@p_nopendaftaran", dts_nopendaftaran);
				command.Parameters.AddWithValue("@p_filetugas", uniqueFileName);
				command.Parameters.AddWithValue("@p_waktupengumpulan", dts_waktupengumpulan);
				command.Parameters.AddWithValue("@p_nilaitugas", dts_nilaitugas);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil diunggah";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Terjadi kesalahan saat mengunggah file: " + ex.Message;
				_response.data = null;
			}

			return _response;
		}

		public ResponseModel UbahTugasMahasiswa(string dts_iddetail, string dts_nopendaftaran, IFormFile file, DateTime dts_waktupengumpulan, double dts_nilaitugas)
		{
			try
			{
				// Mendapatkan direktori tempat menyimpan file
				string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "File_Tugas\\PengumpulanTugas");
				string oldFile = GetOldFileDetail(dts_iddetail, dts_nopendaftaran);
				string oldFilePath = null;

				if (oldFile != null)
				{
					oldFilePath = Path.Combine(uploadDir, oldFile);

					// Mengecek apakah direktori sudah ada, jika belum, maka membuatnya
					if (!Directory.Exists(uploadDir))
					{
						Directory.CreateDirectory(uploadDir);
					}

					// Membuat nama file yang unik untuk menghindari konflik
					string uniqueFileName;

					if (file != null)
					{
						// Jika file tidak null, gunakan nama file baru
						uniqueFileName = file.FileName;

						// Membuat path file tujuan
						string filePath = Path.Combine(uploadDir, uniqueFileName);

						// Menyimpan file ke server
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							file.CopyTo(fileStream);
						}

						// Hapus file lama jika ada
						if (System.IO.File.Exists(oldFilePath))
						{
							System.IO.File.Delete(oldFilePath);
						}
					}
					else
					{
						// Jika file null, gunakan nama file lama
						uniqueFileName = oldFile;
					}

					SqlCommand command = new SqlCommand("sp_UpdateTugasDetail", _connection);
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@p_iddetail", dts_iddetail);
					command.Parameters.AddWithValue("@p_nopendaftaran", dts_nopendaftaran);
					command.Parameters.AddWithValue("@p_filetugas", uniqueFileName);
					command.Parameters.AddWithValue("@p_waktupengumpulan", dts_waktupengumpulan);
					command.Parameters.AddWithValue("@p_nilaitugas", dts_nilaitugas);

					_connection.Open();
					command.ExecuteNonQuery();
					_connection.Close();

					_response.status = 200;
					_response.messages = oldFilePath;
					_response.data = oldFile;
				}
				else
				{
					// Handle the case where oldFile is null (e.g., log a message or take appropriate action)
					_response.status = 500;
					_response.messages = "Terjadi kesalahan: oldFile is null.";
					_response.data = null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Terjadi kesalahan saat mengubah tugas: " + ex.Message;
				_response.data = null;
			}

			return _response;
		}

		public List<Dictionary<string, object>> GetDetailTugasByKelompok(string idtugas, string idkelompok)
		{
			List<Dictionary<string, object>> detailList = new List<Dictionary<string, object>>();
			try
			{
				string query = "SELECT * FROM view_DetailTugasMahasiswa WHERE dts_iddetail = @idtugas AND mhs_idkelompok = @idkelompok";

				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@idtugas", idtugas);
				command.Parameters.AddWithValue("@idkelompok", idkelompok);

				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Dictionary<string, object> detail = new Dictionary<string, object>
					{
						{ "dts_iddetail", reader["dts_iddetail"].ToString() },
						{ "dts_nopendaftaran", reader["dts_nopendaftaran"].ToString() },
						{ "mhs_namalengkap", reader["mhs_namalengkap"].ToString() },
						{ "mhs_gender", reader["mhs_gender"].ToString() },
						{ "mhs_programstudi", reader["mhs_programstudi"].ToString() },
						{ "mhs_kategori", reader["mhs_kategori"].ToString() },
						{ "mhs_idkelompok", reader["mhs_idkelompok"].ToString() },
						{ "dts_filetugas", reader["dts_filetugas"].ToString() },
						{ "dts_waktupengumpulan", DateTime.Parse(reader["dts_waktupengumpulan"].ToString()) },
						{ "dts_nilaitugas", Double.Parse(reader["dts_nilaitugas"].ToString()) },
					};
					detailList.Add(detail);
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

			return detailList;
		}

		public ResponseModel PenilaianTugas(string dts_iddetail, string dts_nopendaftaran, [FromBody] double dts_nilaitugas)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_PenilaianTugas", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_iddetail", dts_iddetail);
				command.Parameters.AddWithValue("@p_nopendaftaran", dts_nopendaftaran);
				command.Parameters.AddWithValue("@p_nilaitugas", dts_nilaitugas);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil dinilai";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Gagal menilai Tugas, " + ex.Message;
				return _response;
			}

			return _response;
		}
	}
}
